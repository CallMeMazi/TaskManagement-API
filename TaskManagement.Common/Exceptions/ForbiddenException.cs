using System.Net;

namespace TaskManagement.Common.Exceptions;
public class ForbiddenException : AppException
{
    public ForbiddenException()
       : base(HttpStatusCode.Forbidden, ResultStatus.Forbidden, "به این بخش دسترسی ندارید!", new() { "به این بخش دسترسی ندارید!" }, null)
    {
    }

    public ForbiddenException(string? message)
        : base(HttpStatusCode.Forbidden, ResultStatus.Forbidden, message, new() { "یافت نشدبه این بخش دسترسی ندارید!" }, null)
    {
    }

    public ForbiddenException(string? message, List<string>? errorMessages)
        : base(HttpStatusCode.Forbidden, ResultStatus.Forbidden, message, errorMessages, null)
    {
    }

    public ForbiddenException(string? message, List<string>? errorMessages, Exception? innerException)
        : base(HttpStatusCode.Forbidden, ResultStatus.Forbidden, message, errorMessages, innerException)
    {
    }

    public ForbiddenException(string? message, Exception? innerException)
        : base(HttpStatusCode.Forbidden, ResultStatus.Forbidden, message, new() { "یافت نشدبه این بخش دسترسی ندارید!" }, innerException)
    {
    }
}
