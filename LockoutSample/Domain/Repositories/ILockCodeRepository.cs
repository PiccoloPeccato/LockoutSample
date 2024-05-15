using LockoutSample.Domain.Entities;

namespace LockoutSample.Domain.Repositories
{
    public interface ILockCodeRepository
    {
        public Task CreateLockCodeAsync(LockCode code, CancellationToken cancellationToken);
        public Task UpdateLockCodeAsync(LockCode code, CancellationToken cancellationToken);
        public Task<LockCode?> GetLockCodeAsync(int userId, CancellationToken cancellationToken);
    }
}
