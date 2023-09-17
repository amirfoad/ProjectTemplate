using Domain.Entities.Authentication.Auditing.ContractAuditng;

namespace Domain.Entities.Authentication.Auditing
{
    public class AuditedEntity<TPrimeryKey> : CreationAuditedEntity<TPrimeryKey>, IAudited
    {
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
    }
}
