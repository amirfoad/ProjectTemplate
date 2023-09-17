using Domain.Entities.Authentication.Auditing;
using Domain.Entities.Authentication.Auditing.ContractAuditng;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication
{

    public class UserLogin : IdentityUserLogin<int>, IEntity
    {
        public UserLogin()
        {
            LoggedOn = DateTime.Now;
        }

        public DateTime LoggedOn { get; set; }
        public int UserId { get; set; }

        #region Navigation Properties

        public User User { get; set; }

        #endregion Navigation Properties
    }


}