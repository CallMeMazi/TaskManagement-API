namespace TaskManagement.Domain.Utilities.Exceptions;
public class DomainValidationFailureException : DomainException
{
    public IReadOnlyCollection<string>? ErrorMessages { get; }

    public DomainValidationFailureException()
        : base("داده های ورودی نامعتبر است!") { }
    public DomainValidationFailureException(string message)
       : base(message) { }
    public DomainValidationFailureException(string message, Exception innerException)
       : base(message, innerException) { }
    public DomainValidationFailureException(string message, IEnumerable<string> errorMessages)
       : base(message)
        => ErrorMessages = errorMessages.ToArray();
    public DomainValidationFailureException(string message, IEnumerable<string> errorMessages, Exception innerException)
       : base(message, innerException)
        => ErrorMessages = errorMessages.ToArray();
}
