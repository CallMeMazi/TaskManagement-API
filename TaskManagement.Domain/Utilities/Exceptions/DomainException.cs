namespace TaskManagement.Domain.Utilities.Exceptions;

public abstract class DomainException(string message, Exception? innerException = null)
    : Exception(message, innerException);
