using Application.Contracts.Authentication;
using Domain.Entities.Authentication;
using Hosumand.KarizWrapper.Identity.Identity.Manager;
using Identity.Identity.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.UserManager
{
    public class AppUserManagerImplementation : IAppUserManager
    {
        private readonly AppUserManager _userManager;
        private readonly AppSignInManager _signInManager;

        public AppUserManagerImplementation(AppUserManager userManager, AppSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<IdentityResult> CreateUser(User user)
        {
            return _userManager.CreateAsync(user);
        }

        public Task<IdentityResult> CreateUserWithPassword(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<bool> IsExistUserName(string userName)
        {
            return _userManager.Users.AnyAsync(c => c.UserName.Equals(userName));
        }

        public Task<bool> IsUserInRoleAsync(User user, string role)
        {
            return _userManager.IsInRoleAsync(user, role);
        }

        public Task<List<User>> GetAllUsers()
        {
            return _userManager.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .Where(x => x.UserRoles.All(c => c.Role.Name != "admin")).ToListAsync();
        }

        public Task<string> GeneratePhoneNumberToken(User user, string phoneNumber)
        {
            return _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public Task<string> GenerateEmailTokenAsync(User user, string email)
        {
            return _userManager.GenerateChangeEmailTokenAsync(user, email);
        }

        public Task<IdentityResult> ChangePhoneNumber(User user, string phoneNumber, string code)
        {
            return _userManager.ChangePhoneNumberAsync(user, phoneNumber, code);
        }

        public Task<IdentityResult> ChangeUserEmailAsync(User user, string email, string token)
        {
            return _userManager.ChangeEmailAsync(user, email, token);
        }

        public Task<string> GeneratePasswordChangeToken(User user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public Task<IdentityResult> ChangePasswordAsync(User user, string passWord, string token)
        {
            return _userManager.ResetPasswordAsync(user, token, passWord);
        }

        public Task<IdentityResult> UpdateUserAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }

        public Task<bool> VerifyUserCode(User user, string code)
        {
            return _userManager.VerifyUserTokenAsync(
                user, "PasswordlessLoginTotpProvider", "passwordless-auth", code);
        }

        public Task<string> GenerateOtpCode(User user)
        {
            return _userManager.GenerateUserTokenAsync(
                 user, "PasswordlessLoginTotpProvider", "passwordless-auth");
        }

        public Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber.Equals(phoneNumber));
        }

        public Task<SignInResult> AdminLogin(User user, string password)
        {
            var result = _signInManager.PasswordSignInAsync(user, password, true, true);
            return result;
        }

        public Task<User> GetByUserName(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public Task<bool> IsExistEmail(string Email)
        {
            return _userManager.Users.AnyAsync(x => x.Email.Equals(Email));
        }

        public Task<User> GetUserById(int UserId)
        {
            return _userManager.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        }

        public Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        public Task<IdentityResult> RemoveFromRoleAsync(User user, string role)
        {
            return _userManager.RemoveFromRoleAsync(user, role);
        }

        public Task<IdentityResult> ChangeSecurityStampAsync(User user)
        {
            return _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}