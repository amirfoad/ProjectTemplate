using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication
{
    public class UserClaim : IdentityUserClaim<int>, IEntity
    {
        #region Navigation Properties

        public User User { get; set; }

        #endregion Navigation Properties
    }
}