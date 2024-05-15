using LockoutSample.Application.Requests;

namespace LockoutSample.Application.Interfaces
{
    public interface ILockoutService
    {
        Task CreateCode(CreateCodeRequest request, CancellationToken cancellationToken);
        Task AccessResource(AccessResourceRequest request, CancellationToken cancellationToken);
    }
}
