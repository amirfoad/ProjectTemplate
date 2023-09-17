namespace Domain.Entities.Authentication.Auditing.ContractAuditng
{
    public interface IDeletionAudited : IHasDeletionTime, ISoftDelete
    {
        long? DeleterUserId { get; set; }
    }
}
