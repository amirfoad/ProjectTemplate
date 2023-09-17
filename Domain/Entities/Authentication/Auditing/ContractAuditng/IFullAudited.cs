namespace Domain.Entities.Authentication.Auditing.ContractAuditng
{
    public interface IFullAudited : IAudited, ICreationAudited, IHasCreationTime, IHasDeletionTime, IDeletionAudited, ISoftDelete
    {
    }
}
