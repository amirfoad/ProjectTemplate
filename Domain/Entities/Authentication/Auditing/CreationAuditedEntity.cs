using Domain.Entities.Authentication.Auditing.ContractAuditng;

namespace Domain.Entities.Authentication.Auditing
{
    public class CreationAuditedEntity<TPrimeryKey> : Entity<TPrimeryKey>, ICreationAudited, IHasCreationTime
    {
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
