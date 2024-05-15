namespace LockoutSample.Domain.Exceptions
{
    public class OutOfAttemptsException : ConflictException
    {
        public OutOfAttemptsException()
            : base("You've run out of attempts. Please try 1 minute later.") { }
    }
}
