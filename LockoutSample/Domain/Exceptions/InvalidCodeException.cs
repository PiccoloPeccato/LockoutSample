namespace LockoutSample.Domain.Exceptions
{
    public class InvalidCodeException : ConflictException
    {
        public InvalidCodeException(int attempts)
            : base($"The entered code is invalid. You have {attempts} attempts remaining.") { }
    }
}
