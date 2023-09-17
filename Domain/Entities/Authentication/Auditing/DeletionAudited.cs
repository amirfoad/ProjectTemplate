using Domain.Entities.Authentication.Auditing.ContractAuditng;

namespace Domain.Entities.Authentication.Auditing
{
    public class DeletionAudited : IDeletionAudited
    {
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
