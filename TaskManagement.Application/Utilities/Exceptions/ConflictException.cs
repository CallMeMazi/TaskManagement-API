using TaskManagement.Application.Utilities.Enums;

namespace TaskManagement.Application.Utilities.Exceptions;
public class ConflictException
    : AppException
{
    public ConflictException()
        : base(ResultStatus.Conflict, "وضعیت نامعتبر است!") { }
    public ConflictException(string message)
        : base(ResultStatus.Conflict, message) { }
    public ConflictException(string message, Exception innerException)
        : base(ResultStatus.Conflict, message, innerException) { }
}
