using TaskManagement.Application.Utilities.Enums;

namespace TaskManagement.Application.Utilities.Exceptions;
public class UnAuthorizedException : AppException
{
    public UnAuthorizedException()
        : base(ResultStatus.Unauthorized, "شما دسترسی ندارید!") { }
    public UnAuthorizedException(string message)
        : base(ResultStatus.Unauthorized, message) { }
    public UnAuthorizedException(string message, Exception innerException)
        : base(ResultStatus.Unauthorized, message, innerException) { }
}
