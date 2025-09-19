using System.Net;

namespace TaskManagement.Common.Exceptions;
public class NotFoundException : AppException
{
    public NotFoundException() 
        : base(HttpStatusCode.NotFound, ResultStatus.NotFound, "یافت نشد!", new string[] { "یافت نشد!" }, null)
    {
    }

    public NotFoundException(string? message)
        : base(HttpStatusCode.NotFound, ResultStatus.NotFound, message, new string[] { "یافت نشد!" }, null)
    {
    }

    public NotFoundException(string? message, string[]? errorMessages)
        : base(HttpStatusCode.NotFound, ResultStatus.NotFound, message, errorMessages, null)
    {
    }

    public NotFoundException(string? message, string[]? errorMessages, Exception? innerException)
        : base(HttpStatusCode.NotFound, ResultStatus.NotFound, message, errorMessages, innerException)
    {
    }

    public NotFoundException(string? message, Exception? innerException)
        : base(HttpStatusCode.NotFound, ResultStatus.NotFound, message, new string[] { "یافت نشد!" }, innerException)
    {
    }
}
