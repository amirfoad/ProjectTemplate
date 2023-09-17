namespace Domain.Entities.Authentication.Auditing.ContractAuditng
{
    public interface IEntity<TPrimeryKey>
    {
        public TPrimeryKey Id { get; set; }
    }
}
