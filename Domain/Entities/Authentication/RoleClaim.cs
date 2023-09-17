using Microsoft.AspNetCore.Identity;
namespace Domain.Entities.Authentication
{
    public class RoleClaim : IdentityRoleClaim<int>, IEntity
    {
        public RoleClaim()
        {
            CreatedClaim = DateTime.Now;
        }

        public DateTime CreatedClaim { get; set; }

        #region Navigation Properties

        public Role Role { get; set; }

        #endregion Navigation Properties
    }
}