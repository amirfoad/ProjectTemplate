using Application.Contracts.Persistence.UserContracts;

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        public IUserRefreshTokenRepository UserRefreshTokenRepository { get; }

        public IUserRepository UserRepository { get; }

        Task CommitAsync();

        ValueTask RollBackAsync();
    }
}