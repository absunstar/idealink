using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Employment.ModelsGlobal;

namespace Tadrebt.STS.Quickstart.Account
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            //var user = await _userManager.FindByIdAsync(sub);

            var email = context.Subject.Identity.Name;
            var user = await _userManager.FindByEmailAsync(email);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();

            //https://deblokt.com/2019/09/27/05-identityserver4-adding-custom-properties-to-user/
            claims.Add(new Claim("MDID", user.MDID != null ? user.MDID : "5f32e1756ffbac6478b7e944"));


            var tmpClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            claims.Remove(tmpClaim);

            claims.Add(new Claim(JwtClaimTypes.Subject, sub));
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
