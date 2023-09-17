using Application.Contracts.Persistence.UserContracts;
using Domain.Entities.Authentication;
using Persistence.Context;

namespace Persistence.Repositories.UserRepo
{
    public class UserRepository : BaseAsyncRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        IQueryable<User> IUserRepository.TableNoTracking => base.TableNoTracking;

        IQueryable<User> IUserRepository.Table => base.Table;
    }
}