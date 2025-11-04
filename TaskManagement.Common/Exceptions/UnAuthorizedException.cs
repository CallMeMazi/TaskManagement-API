using System.Net;
using TaskManagement.Common.Enums;

namespace TaskManagement.Common.Exceptions;
public class UnAuthorizedException : AppException
{
    public UnAuthorizedException()
        : base(HttpStatusCode.Unauthorized, ResultStatus.UnAuthorized, null, null, null)
    {
    }

    public UnAuthorizedException(string? message)
        : base(HttpStatusCode.Unauthorized, ResultStatus.UnAuthorized, message, new() { message ?? "مشکلی پیش آمده!" }, null)
    {
    }

    public UnAuthorizedException(string? message, List<string>? errorMessages)
        : base(HttpStatusCode.Unauthorized, ResultStatus.UnAuthorized, message, errorMessages, null)
    {
    }

    public UnAuthorizedException(string? message, List<string>? errorMessages, Exception? innerException)
        : base(HttpStatusCode.Unauthorized, ResultStatus.UnAuthorized, message, errorMessages, innerException)
    {
    }

    public UnAuthorizedException(string? message, Exception? innerException)
        : base(HttpStatusCode.Unauthorized, ResultStatus.UnAuthorized, message, new() { message ?? "مشکلی پیش آمده!" }, innerException)
    {
    }
}
