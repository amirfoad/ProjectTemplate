namespace Domain.Entities.Authentication
{
    public class UserRefreshToken : BaseEntity<Guid>
    {
        public UserRefreshToken()
        {
            CreatedAt = DateTime.Now;
        }

        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsValid { get; set; }

        #region Navigation Properties

        public User User { get; set; }

        #endregion Navigation Properties
    }
}