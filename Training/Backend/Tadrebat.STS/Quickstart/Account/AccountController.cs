// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tadrebat.ModelsGlobal;
using Tadrebat.STS;
using Flurl;
using Tadrebat.Interface;
using Tadrebat.Enum;
using iTextSharp.text.pdf.qrcode;
using System.IO;
using System.Reflection;

namespace IdentityServer4.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IUserManagement BLUserManagement;
        private readonly string baseURL;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserManagement _userManagement)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            BLUserManagement = _userManagement;

            ViewBag.ClientURL = Config.urlSPAClient;
            baseURL = Config.urlSPAClient + "";
            baseURL = Flurl.Url.Combine(baseURL, "home");
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            ViewBag.MessageSuccess = TempData["MessageSuccess"];

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }


            try
            {
                if (ModelState.IsValid)
                {

                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user == null || user.EmailConfirmed)
                    {

                        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.ClientId));

                            if (string.IsNullOrEmpty(model.ReturnUrl))
                                model.ReturnUrl = Config.urlSPAClient;

                            if (context != null)
                            {
                                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                                {
                                    // if the client is PKCE then we assume it's native, so this change in how to
                                    // return the response is for better UX for the end user.
                                    return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                                }

                                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                                return Redirect(model.ReturnUrl);
                            }

                            // request for a local page
                            if (Url.IsLocalUrl(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }
                            else if (string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return Redirect("~/");
                            }
                            else
                            {
                                // user might have clicked on a malicious link - should be logged
                                throw new Exception("invalid return URL");
                            }
                        }
                    }

                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                }
            }
            catch (Exception ex)
            {
                LogWrite("Exception - " + ex);
            }



            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }


        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }
            //vm.PostLogoutRedirectUri = Config.urlSPAClient;
            if (vm.AutomaticRedirectAfterSignOut && !string.IsNullOrWhiteSpace(vm.PostLogoutRedirectUri))
            {
                return Redirect(vm.PostLogoutRedirectUri);
            }
            else
            {
                return View("LoggedOut", vm);
            }

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        //http://www.binaryintellect.net/articles/6c463905-ed70-4b61-a05d-94083bfbec66.aspx
        public async Task<object> CreateSTSUser(string Email, EnumUserTypes Type, string MDID)
        {
            if (string.IsNullOrEmpty(Email) || Type == 0 || string.IsNullOrEmpty(MDID))
                return BadRequest();

            ApplicationUser user = new ApplicationUser();
            user.Email = Email;
            user.UserName = Email;
            user.MDID = MDID;
            var result = await BLUserManagement.CreateApplicationUser(user, Type);

            return result;
        }
        public async Task<object> ResetSTSPassword(string Email, string NewPassword, string OldPassword)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(OldPassword))
                return BadRequest();

            var result = await BLUserManagement.ResetPassword(Email, NewPassword, OldPassword);

            return result;
        }
        public async Task<IActionResult> ConfirmAccount(string Token, Guid UserId)
        {
            string url = "";
            if (string.IsNullOrEmpty(Token) || UserId == Guid.Empty || UserId == null)
                return BadRequest();

            if (await BLUserManagement.IsEmailConfirmed(UserId))
            {
                url = Flurl.Url.Combine(baseURL, "Email already confirmed.", "false");
                return Redirect(url);
            }
            //var result = await BLUserManagement.ConfirmAccount(Token, UserId);

            //if (result)
            //{
            //    var token = await BLUserManagement.GenerateUserPasswordToken(UserId);
            //    return RedirectToAction("SetPassword", new { Token = token, UserId = UserId });
            //}
            var tokenPassword = await BLUserManagement.GenerateUserPasswordToken(UserId);
            return RedirectToAction("SetPassword", new { Token = tokenPassword, UserId = UserId, tokenConfirm = Token });

            //var msg = result ? "“Email Confirmation Success." : "Email Confirmation Failed";
            //url = Flurl.Url.Combine(baseURL, msg, result.ToString().ToLower());

            //return Redirect(url);
        }
        public async Task<IActionResult> SetPassword(string Token, Guid UserId, string tokenConfirm)
        {
            //ModelSetPassword model = new ModelSetPassword();
            //model.ResetToken = "";
            //model.UserID = Guid.NewGuid();
            //return View(model);

            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(Token) || UserId == Guid.Empty)
            {
                var url = Flurl.Url.Combine(baseURL, "Setting Password Failed", "false");
                return Redirect(url);
            }

            ModelSetPassword model = new ModelSetPassword();
            model.ResetToken = Token;
            model.UserID = UserId;
            model.tokenConfirm = tokenConfirm;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(ModelSetPassword model)
        {
            string url = "";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.UserID.ToString());

            if (!user.EmailConfirmed)
            {
                var resultConfirm = await BLUserManagement.ConfirmAccount(model.tokenConfirm, model.UserID);

                if (!resultConfirm)
                {
                    url = Flurl.Url.Combine(baseURL, "Email Confirmation Failed", "false");
                    return Redirect(url);
                }
            }
            var result = await BLUserManagement.SetPassword(model.NewPassword, model.UserID, model.ResetToken);
            if (result == null)
            {
                url = Flurl.Url.Combine(baseURL, "Set password has failed.", "false");
                return Redirect(url);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Errors.ToList()[0].Description);
                return View(model);
            }
            url = Flurl.Url.Combine(baseURL, "Set password has succeeded.", "True");
            return Redirect(url);

        }
        public async Task<IActionResult> ForgetPasswordHome()
        {
            return View("ForgetPassword");
        }
        public async Task<IActionResult> ForgetPassword(ModelForgetPassword model, string button)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (button != "ForgetPassword")
            {
                return RedirectToAction("Login");
            }
            if (!await BLUserManagement.ForgetPassword(model.Email))
            {
                //ModelState.AddModelError(string.Empty, "Could not reset password for this user");
                //return View(model);
            }
            TempData["MessageSuccess"] = "Password reset link has been sent to your email address.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ResendVerify(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (!user.EmailConfirmed)
            {
                await BLUserManagement.ResendVerify(user.Email);
                TempData["MessageSuccess"] = "Activation link is sent successfully to your email.";
            }
            return RedirectToAction("Login");
        }
        public async Task<object> ResendActivationLink(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return BadRequest();

            if (user.EmailConfirmed)
            {
                return BadRequest();
            }
            await BLUserManagement.ResendVerify(user.Email);
            return Ok();
        }
        public async Task<object> ResendPasswordLink(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return BadRequest();

            if (!user.EmailConfirmed)
            {
                return BadRequest();
            }
            await BLUserManagement.ForgetPassword(user.Email);
            return Ok();
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        public void LogWrite(string logMessage)
        {
            string m_exePath = string.Empty;
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = new StreamWriter(m_exePath + "\\" + "log.txt", true))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}