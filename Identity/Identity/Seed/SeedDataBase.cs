using Application.Contracts.Persistence;
using Domain.Entities.Authentication;
using Identity.Identity.Manager;
using Microsoft.EntityFrameworkCore;

namespace Identity.Identity.Seed
{
    public interface ISeedDataBase
    {
        Task Seed();
    }

    public class SeedDataBase : ISeedDataBase
    {
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public SeedDataBase(AppUserManager userManager, AppRoleManager roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task Seed()
        {
            if (!_roleManager.Roles.AsNoTracking().Any(r => r.Name.Equals("admin")))
            {
                var role = new Role
                {
                    Name = "admin",
                };
                await _roleManager.CreateAsync(role);
            }
            if (!_roleManager.Roles.AsNoTracking().Any(r => r.Name.Equals("verifiedUser")))
            {
                var role = new Role
                {
                    Name = "verifiedUser",
                };
                await _roleManager.CreateAsync(role);
            }
            if (!_roleManager.Roles.AsNoTracking().Any(r => r.Name.Equals("unverifiedUser")))
            {
                var role = new Role
                {
                    Name = "unverifiedUser",
                };
                await _roleManager.CreateAsync(role);
            }

            await _unitOfWork.CommitAsync();

            if (!_userManager.Users.AsNoTracking().Any(u => u.UserName.Equals("admin")))
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@site.com",
                    PhoneNumberConfirmed = true,
                    PhoneNumber = "09121111111",
                };

                await _userManager.CreateAsync(user, "qw123321");
                await _userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}