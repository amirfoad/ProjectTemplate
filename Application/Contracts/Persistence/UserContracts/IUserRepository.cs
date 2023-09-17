using Domain.Entities.Authentication;

namespace Application.Contracts.Persistence.UserContracts
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        IQueryable<User> TableNoTracking { get; }
        IQueryable<User> Table { get; }
    }
}