namespace Domain.Entities.Authentication.Auditing.ContractAuditng
{
    public interface IHasDeletionTime : ISoftDelete
    {
        DateTime? DeletionTime { get; set; }
    }
}
