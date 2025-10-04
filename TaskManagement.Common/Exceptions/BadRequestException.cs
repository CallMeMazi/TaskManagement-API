using System.Net;
using TaskManagement.Common.Enums;

namespace TaskManagement.Common.Exceptions;
public class BadRequestException : AppException
{
    public BadRequestException()
        : base(HttpStatusCode.BadRequest, ResultStatus.BadRequest, null, null, null)
    {
    }

    public BadRequestException(string? message)
        : base(HttpStatusCode.BadRequest, ResultStatus.BadRequest, message, new() { message ?? "مشکلی پیش آمده!"}, null)
    {
    }

    public BadRequestException(string? message, List<string>? errorMessages)
        : base(HttpStatusCode.BadRequest, ResultStatus.BadRequest, message, errorMessages, null)
    {
    }

    public BadRequestException(string? message, List<string>? errorMessages, Exception? innerException)
        : base(HttpStatusCode.BadRequest, ResultStatus.BadRequest, message, errorMessages, innerException)
    {
    }

    public BadRequestException(string? message, Exception? innerException)
        : base(HttpStatusCode.BadRequest, ResultStatus.BadRequest, message, new() { message ?? "مشکلی پیش آمده!" }, innerException)
    {
    }
}
