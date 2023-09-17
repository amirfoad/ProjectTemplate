namespace Domain.Entities.Authentication.Auditing.ContractAuditng
{
    public interface ICreationAudited
    {
        public long? CreatorUserId { get; set; }
    }
}
