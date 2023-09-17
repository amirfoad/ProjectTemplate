namespace Domain.Entities.Authentication.Auditing.ContractAuditng
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
