using Application.Dtos.Jwt;
using Domain.Entities.Authentication;
using System.Security.Claims;

namespace Application.Contracts.Authentication
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user, int actorId = 0);

        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);

        Task<AccessToken> GenerateByPhoneNumberAsync(string phoneNumber);

        Task<AccessToken> RefreshToken(string refreshTokenId, int actorId = 0);
    }
}