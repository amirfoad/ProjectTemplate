using Domain.Entities.Authentication;
using Identity.Identity.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.Identity
{
    public class AppUserClaimsPrincipleFactory : UserClaimsPrincipalFactory<User, Role>, IAppUserPrincipalFactory
    {
        public AppUserClaimsPrincipleFactory(AppUserManager userManager, AppRoleManager roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        protected async Task<ClaimsIdentity> GenerateClaimsAsync(User user, int actorId)
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            var claimsIdentity = await base.GenerateClaimsAsync(user);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Actor, actorId.ToString(), ClaimValueTypes.Integer));
            claimsIdentity.AddClaim(new Claim("ChannelId", user.ChannelId.ToString()));
            return claimsIdentity;
        }

        public async Task<ClaimsPrincipal> CreateAsync(User user, int actorId = 0)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var id = await GenerateClaimsAsync(user, actorId);
            return new ClaimsPrincipal(id);
        }
    }
}