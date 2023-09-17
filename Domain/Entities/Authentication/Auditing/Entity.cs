using Domain.Entities.Authentication.Auditing.ContractAuditng;

namespace Domain.Entities.Authentication.Auditing
{
    public class Entity<TPrimeryKey> : IEntity<TPrimeryKey>
    {
        public virtual TPrimeryKey Id
        {
            get;
            set;
        }


        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
