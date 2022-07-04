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
using Employment.ModelsGlobal;
using Employment.STS;
using Flurl;
using Employment.Interface;
using Employment.Enum;
using System.Security.Claims;
using System.Security.Principal;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Text;
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
        private readonly string baseURL;
        private readonly TestUserStore _users;

        private readonly IUserManagement BLUserManagement;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserManagement _userManagement,

            TestUserStore users = null)
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

            // if the TestUserStore is not in DI, then we'll just use the global users collection
            _users = users ?? new TestUserStore(new List<TestUser>());
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
            //  var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            //model.ReturnUrl = "http://sts.ms-employment.digisummits.com/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3D87a10b87-XXXX-9457-083ed25faac5%26redirect_uri%3Dhttps%253A%252F%252Femployment.idealake.com%252Fsignin-callback%26response_type%3Dcode%26scope%3Dopenid%2520profile%2520projects-api%26state%3D1ccb296ff522427d8a9ae8d5ea9b3310%26code_challenge%3DgyGi03x1WDEtxhCB8VUcCwLN75DCQWOE-1WL-PUIyQ0%26code_challenge_method%3DS256%26response_mode%3Dquery";
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            // var context = await _interaction.GetAuthorizationContextAsync("/connect/authorize/callback?client_id=87a10b87-XXXX-9457-083ed25faac5");
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

                    return Redirect(baseURL);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect(baseURL);
                }
            }

            if (ModelState.IsValid
                && SimpleCaptcha.Validator.Validate(model.CaptchaInput, HttpContext)) //validate captcha
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && user.IsApproved)
                {
                    if (!user.EmailConfirmed)
                    {
                        await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                        //ModelState.AddModelError(string.Empty, AccountOptions.InactiveAccountErrorMessage.Replace("####", user.Id));
                        ViewBag.UserId = user.Id;
                        return View(await BuildLoginViewModelAsync(model));
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.ClientId));

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
                }
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                return View(await BuildLoginViewModelAsync(model));
            }
            ModelState.AddModelError(string.Empty, "Please enter correct captcha.");
            // something went wrong, show form with error
            return View(await BuildLoginViewModelAsync(model));
        }



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
        /// <summary>
        /// Show logout page
        /// </summary>
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
            vm.PostLogoutRedirectUri = Config.urlSPAClient;
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
        public async Task<object> CreateSTSUser(string Email, EnumUserTypes Type, string MDID, string password)
        {
            if (string.IsNullOrEmpty(Email) || Type == 0 || string.IsNullOrEmpty(MDID))
                return BadRequest();

            ApplicationUser user = new ApplicationUser();
            user.Email = Email;
            user.UserName = Email;
            user.MDID = MDID;
            user.IsApproved = true;

            var result = await BLUserManagement.CreateApplicationUser(user, Type, password);

            return result;
        }
        public async Task<object> UpdateUserRole(string Email, EnumUserTypes Type)
        {
            if (string.IsNullOrEmpty(Email) || Type == 0)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return BadRequest();

            var result = await BLUserManagement.UpdateUndefinedUserToRole(user, Type);

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

            var result = await BLUserManagement.ConfirmAccount(Token, UserId);

            if (result)
            {
                var user = await _userManager.FindByIdAsync(UserId.ToString());
                if (await _userManager.IsInRoleAsync(user, "admin"))
                {
                    var token = await BLUserManagement.GenerateUserPasswordToken(UserId);
                    return RedirectToAction("SetPassword", new
                    {
                        Token = token,
                        UserId = UserId
                    });
                    ;
                }
            }
            var msg = result ? "Email confirmation successed." : "Email confirmation failed.";
            url = Flurl.Url.Combine(baseURL, msg, result.ToString().ToLower());

            return Redirect(url);
        }
        public async Task<IActionResult> AccountStatus(string email, bool Status)
        {
            string url = "";
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(email);
            user.IsApproved = Status;

            await _userManager.UpdateAsync(user);

            return Ok();
        }
        public async Task<IActionResult> SetPassword(string Token, Guid UserId)
        {
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
            var result = await BLUserManagement.SetPassword(model.NewPassword, model.UserID, model.ResetToken);
            if (result == null)
            {
                url = Flurl.Url.Combine(baseURL, "Email confirmation failed", "false");
                return Redirect(url);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Errors.ToList()[0].Description);
                return View(model);
            }
            url = Flurl.Url.Combine(baseURL, "Email confirmation success", "True");
            return Redirect(url);

        }
        [HttpGet]
        public async Task<IActionResult> ChangeUserPassword(ModelChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var result = await BLUserManagement.ChangePassword(model.OldPassword, model.NewPassword, model.Email);
            if (!result.Succeeded)
                return BadRequest(result.Errors.ToList()[0].Description);

            return Ok(result);
        }
        public async Task<IActionResult> ForgetPasswordHome()
        {
            return View("ForgetPassword");
        }
        public async Task<IActionResult> ForgetPassword(ModelForgetPassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            switch (await BLUserManagement.ForgetPassword(model.Email))
            {
                case ResponseForgetPassword.Failed:
                default:
                    ModelState.AddModelError(string.Empty, "Could not reset password for this user");
                    return View(model);
                case ResponseForgetPassword.Success:
                    TempData["MessageSuccess"] = "Password reset link has been sent to your email address!";
                    return RedirectToAction("Login");
                case ResponseForgetPassword.ConfirmResend:
                    ModelState.AddModelError(string.Empty, "User accont is not confirmed. A new confirmation has been resent.");
                    return View(model);
            }

        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("Callback", "External"),
                // RedirectUri = Url.Action("ExternalLoginCallback"),
                Items =
                {
                    { "returnUrl", returnUrl }
                }
            };

            // windows authentication needs special handling
            // since they don't support the redirect uri, 
            // so this URL is re-triggered when we call challenge
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // see if windows auth has already been requested and succeeded
                var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
                if (result?.Principal is WindowsPrincipal wp)
                {
                    props.Items.Add("scheme", AccountOptions.WindowsAuthenticationSchemeName);

                    var id = new ClaimsIdentity(provider);
                    id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                    id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                    // add the groups as claims -- be careful if the number of groups is too large
                    if (AccountOptions.IncludeWindowsGroups)
                    {
                        var wi = wp.Identity as WindowsIdentity;
                        var groups = wi.Groups.Translate(typeof(NTAccount));
                        var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                        id.AddClaims(roles);
                    }

                    await HttpContext.SignInAsync(
                        IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                        new ClaimsPrincipal(id),
                        props);
                    return Redirect(props.RedirectUri);
                }
                else
                {
                    // challenge/trigger windows auth
                    return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
                }
            }
            else
            {
                // start challenge and roundtrip the return URL
                props.Items.Add("scheme", provider);
                return RedirectToAction("Challenge", "external", new
                {
                    provider = provider,
                    returnUrl = returnUrl
                });
                //return Challenge(props, provider);
            }
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        //public async Task<IActionResult> ExternalLoginCallback()
        //{
        //    // read external identity from the temporary cookie
        //    var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
        //    if (result?.Succeeded != true)
        //    {
        //        throw new Exception("External authentication error");
        //    }

        //    // retrieve claims of the external user
        //    var externalUser = result.Principal;
        //    var claims = externalUser.Claims.ToList();

        //    // try to determine the unique id of the external user (issued by the provider)
        //    // the most common claim type for that are the sub claim and the NameIdentifier
        //    // depending on the external provider, some other claim type might be used
        //    var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
        //    if (userIdClaim == null)
        //    {
        //        userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        //    }
        //    if (userIdClaim == null)
        //    {
        //        throw new Exception("Unknown userid");
        //    }

        //    // remove the user id claim from the claims collection and move to the userId property
        //    // also set the name of the external authentication provider
        //    claims.Remove(userIdClaim);
        //    var provider = result.Properties.Items["scheme"];
        //    var userId = userIdClaim.Value;

        //    // this is where custom logic would most likely be needed to match your users from the
        //    // external provider's authentication result, and provision the user as you see fit.
        //    // 
        //    // check if the external user is already provisioned
        //    var user = _users.FindByExternalProvider(provider, userId);
        //    if (user == null)
        //    {
        //        // this sample simply auto-provisions new external user
        //        // another common approach is to start a registrations workflow first
        //        user = _users.AutoProvisionUser(provider, userId, claims);
        //    }

        //    // remove 'name' claim issued by idsrv
        //    user.Claims.Remove(user.Claims
        //        .SingleOrDefault(c => c.Type.Equals("name") && c.Issuer.Equals("LOCAL AUTHORITY")));

        //    var additionalClaims = new List<Claim>();

        //    // if the external system sent a session id claim, copy it over
        //    // so we can use it for single sign-out
        //    var sid = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        //    if (sid != null)
        //    {
        //        additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        //    }

        //    // if the external provider issued an id_token, we'll keep it for signout
        //    AuthenticationProperties props = null;
        //    var id_token = result.Properties.GetTokenValue("id_token");
        //    if (id_token != null)
        //    {
        //        props = new AuthenticationProperties();
        //        props.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
        //    }

        //    // issue authentication cookie for user
        //    await _events.RaiseAsync(new UserLoginSuccessEvent(provider, userId, user.SubjectId, user.Username));
        //    await HttpContext.SignInAsync(user.SubjectId, user.Username, provider, props, additionalClaims.ToArray());

        //    // delete temporary cookie used during external authentication
        //    await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);

        //    // validate return URL and redirect back to authorization endpoint or a local page
        //    var returnUrl = result.Properties.Items["returnUrl"];
        //    if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
        //    {
        //        await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: "spa-codingofemployment-client"));
        //        return View("Redirect", new RedirectViewModel { RedirectUrl = "http://www.ms-employment.digisummits.com/" });
        //        return Redirect(returnUrl);
        //    }

        //    return Redirect("~/");
        //}
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
    }
}