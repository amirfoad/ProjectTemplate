using Domain.Entities.Authentication.Auditing.ContractAuditng;

namespace Domain.Entities.Authentication.Auditing
{
    public class HasDeletionTime : IHasDeletionTime
    {
        public DateTime? DeletionTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
