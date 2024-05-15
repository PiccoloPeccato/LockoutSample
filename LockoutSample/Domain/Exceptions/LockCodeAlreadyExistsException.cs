namespace LockoutSample.Domain.Exceptions
{
    public class LockCodeAlreadyExistsException : ConflictException
    {
        public LockCodeAlreadyExistsException(int userId)
            : base($"The lock code for the user {userId} already exists") { }
    }
}
