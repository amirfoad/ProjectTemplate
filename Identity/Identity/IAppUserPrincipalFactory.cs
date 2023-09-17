using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Identity
{
    public interface IAppUserPrincipalFactory : IUserClaimsPrincipalFactory<User>
    {
        Task<ClaimsPrincipal> CreateAsync(User user, int requestingAdminId = 0);
    }
}