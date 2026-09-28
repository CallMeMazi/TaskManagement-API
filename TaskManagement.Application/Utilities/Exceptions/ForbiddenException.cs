using TaskManagement.Application.Utilities.Enums;

namespace TaskManagement.Application.Utilities.Exceptions;
public class ForbiddenException
    : AppException
{
    public ForbiddenException()
        : base(ResultStatus.Forbidden, "دسترسی ندارید!") { }
    public ForbiddenException(string message)
        : base(ResultStatus.Forbidden, message) { }
    public ForbiddenException(string message, Exception innerException)
        : base(ResultStatus.Forbidden, message, innerException) { }
}
