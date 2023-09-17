using Domain.Entities.Authentication.Auditing.ContractAuditng;

namespace Domain.Entities.Authentication.Auditing
{
    public class HasCreationTime : IHasCreationTime
    {
        public DateTime CreationTime { get; set; }
    }
}
