using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication
{
    public class UserRole : IdentityUserRole<int>, IEntity
    {
        #region Navigation Properties

        public User User { get; set; }
        public Role Role { get; set; }

        #endregion Navigation Properties
    }
}