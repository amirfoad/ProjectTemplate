using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication
{
    public class Role : IdentityRole<int>, IEntity
    {
        public string Description { get; set; }

        #region Navigation Properties

        public ICollection<RoleClaim> Claims { get; set; }
        public ICollection<UserRole> Users { get; set; }

        #endregion Navigation Properties
    }
}