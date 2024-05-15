using LockoutSample.Domain.Entities;
using LockoutSample.Domain.Repositories;
using LockoutSample.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LockoutSample.Infrastructure.Repositories
{
    public class LockCodeRepository(LockoutDbContext context) : ILockCodeRepository
    {
        public async Task CreateLockCodeAsync(LockCode code, CancellationToken cancellationToken)
        {
            using var transaction = context.Database.BeginTransaction();

            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Codes ON;", cancellationToken);

            await context.Codes.AddAsync(code, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Codes OFF;", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        public Task<LockCode?> GetLockCodeAsync(int userId, CancellationToken cancellationToken)
        {
            return context.Codes.FirstOrDefaultAsync
                (code => code.UserId == userId, cancellationToken);
        }

        public Task UpdateLockCodeAsync(LockCode code, CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
