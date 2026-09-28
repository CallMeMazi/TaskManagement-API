namespace TaskManagement.Domain.Utilities.Exceptions;
public class DomainLogicalException : DomainException
{
    public DomainLogicalException()
    : base("درخواست شما با وضعیت کنونی قابل انجام نیست!") { }
    public DomainLogicalException(string message)
       : base(message) { }
    public DomainLogicalException(string message, Exception innerException)
       : base(message, innerException) { }
}
