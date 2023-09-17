using Application.Contracts.Persistence;
using Application.Contracts.Persistence.UserContracts;
using Persistence.Context;
using Persistence.Repositories.UserRepo;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IUserRefreshTokenRepository UserRefreshTokenRepository { get; }

        public IUserRepository UserRepository { get; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UserRefreshTokenRepository = new UserRefreshTokenRepository(_db);
            UserRepository = new UserRepository(_db);
        }

        public Task CommitAsync()
        {
            return _db.SaveChangesAsync();
        }

        public ValueTask RollBackAsync()
        {
            return _db.DisposeAsync();
        }
    }
}