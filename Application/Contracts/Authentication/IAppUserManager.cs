using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Authentication
{
    public interface IAppUserManager
    {
        Task<IdentityResult> CreateUser(User user);

        Task<User> GetUserByRefreshToken(string refreshToken);

        Task<IdentityResult> CreateUserWithPassword(User user, string password);

        Task<bool> IsExistEmail(string Email);

        Task<bool> IsExistUserName(string userName);

        Task<bool> IsUserInRoleAsync(User user, string role);

        Task<List<User>> GetAllUsers();

        Task<string> GeneratePhoneNumberToken(User user, string phoneNumber);

        Task<string> GenerateEmailTokenAsync(User user, string email);

        Task<IdentityResult> ChangePhoneNumber(User user, string phoneNumber, string code);

        Task<IdentityResult> ChangeUserEmailAsync(User user, string email, string token);

        Task<string> GeneratePasswordChangeToken(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string passWord, string token);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<bool> VerifyUserCode(User user, string code);

        Task<string> GenerateOtpCode(User user);

        Task<User> GetUserByPhoneNumber(string phoneNumber);

        Task<SignInResult> AdminLogin(User user, string password);

        Task<User> GetByUserName(string userName);

        Task<User> GetUserById(int UserId);

        Task<IdentityResult> AddToRoleAsync(User user, string role);

        Task<IdentityResult> RemoveFromRoleAsync(User user, string role);

        Task<IdentityResult> ChangeSecurityStampAsync(User user);
    }
}