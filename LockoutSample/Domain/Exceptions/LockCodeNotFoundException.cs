namespace LockoutSample.Domain.Exceptions
{
    public class LockCodeNotFoundException : NotFoundException
    {
        public LockCodeNotFoundException(int userId)
            : base($"The lock code for the user {userId} was not found") { }
    }
}
