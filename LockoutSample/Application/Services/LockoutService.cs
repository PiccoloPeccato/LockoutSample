using LockoutSample.Application.Interfaces;
using LockoutSample.Application.Requests;
using LockoutSample.Domain.Entities;
using LockoutSample.Domain.Exceptions;
using LockoutSample.Domain.Repositories;

namespace LockoutSample.Application.Services
{
    public class LockoutService(ILockCodeRepository lockCodeRepository) : ILockoutService
    {
        public async Task AccessResource(AccessResourceRequest request, CancellationToken cancellationToken)
        {
            LockCode? code = await lockCodeRepository.GetLockCodeAsync
                (request.UserId, cancellationToken);

            if (code == null)
            {
                throw new LockCodeNotFoundException(request.UserId);
            }

            var now = DateTime.UtcNow;

            if (now < code.LockoutEnd)
            {
                throw new OutOfAttemptsException();
            }

            const int DefaultAttempts = 3;

            if (code.Code == request.Code)
            {
                code.Attempts = DefaultAttempts;

                await lockCodeRepository.UpdateLockCodeAsync(code, cancellationToken);

                return;
            }

            if (code.Attempts == 1)
            {
                code.Attempts = DefaultAttempts;
                code.LockoutEnd = now + TimeSpan.FromMinutes(1);

                await lockCodeRepository.UpdateLockCodeAsync(code, cancellationToken);

                throw new OutOfAttemptsException();
            }

            if (code.Attempts > 1)
            {
                code.Attempts--;

                await lockCodeRepository.UpdateLockCodeAsync(code, cancellationToken);

                throw new InvalidCodeException(code.Attempts);
            }
        }

        public async Task CreateCode(CreateCodeRequest request, CancellationToken cancellationToken)
        {
            LockCode? code = await lockCodeRepository.GetLockCodeAsync
                (request.UserId, cancellationToken);

            if (code != null)
            {
                throw new LockCodeAlreadyExistsException(request.UserId);
            }

            code = new LockCode()
            {
                UserId = request.UserId,
                Code = request.Code,
                Attempts = 3
            };

            await lockCodeRepository.CreateLockCodeAsync(code, cancellationToken);
        }
    }
}
