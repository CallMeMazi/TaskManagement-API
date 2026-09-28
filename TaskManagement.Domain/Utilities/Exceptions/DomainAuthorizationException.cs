namespace TaskManagement.Domain.Utilities.Exceptions;
public class DomainAuthorizationException : DomainException
{
    public DomainAuthorizationException()
        : base("شما به این عمل دسترسی ندارید!") { }
    public DomainAuthorizationException(string message)
       : base(message) { }
    public DomainAuthorizationException(string message, Exception innerException)
       : base(message, innerException) { }
}
