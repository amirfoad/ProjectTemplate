using Domain.Entities.Authentication;

namespace Application.Contracts.Token
{
    public interface ITokenAppService
    {
        Task<string> GenerateToken(User user);
    }
}