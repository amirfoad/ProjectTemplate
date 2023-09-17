using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence.Context;

namespace Identity.Identity.Store
{
    public class RoleStore : RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>
    {
        public RoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}