using System.Threading.Channels;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication
{
    public class User : IdentityUser<int>, ITimeModification, IEntity
    {
        public int ChannelId { get; set; }
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #region Navigation Properties
        
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<UserToken> Tokens { get; set; }

        #endregion Navigation Properties
    }
}