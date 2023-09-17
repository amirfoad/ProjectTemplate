using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication
{
    public class UserToken : IdentityUserToken<int>, IEntity
    {
        public UserToken()
        {
            GeneratedTime = DateTime.Now;
        }

        public DateTime GeneratedTime { get; set; }

        #region Navigation Properties

        public User User { get; set; }

        #endregion Navigation Properties
    }
}