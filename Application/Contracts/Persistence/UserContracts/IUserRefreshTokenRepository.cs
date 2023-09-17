using Domain.Entities.Authentication;

namespace Application.Contracts.Persistence.UserContracts
{
    public interface IUserRefreshTokenRepository
    {
        Task<Guid> CreateToken(int userId);

        Task<UserRefreshToken> GetTokenWithInvalidation(Guid id);

        Task<User> GetUserByRefreshToken(Guid tokenId);

        Task RemoveUserOldTokens(int userId);

        Task RemoveTokensAsync(int userId, CancellationToken cancellationToken);
    }
}