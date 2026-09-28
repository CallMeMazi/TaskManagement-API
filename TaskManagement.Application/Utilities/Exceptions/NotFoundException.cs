using TaskManagement.Application.Utilities.Enums;

namespace TaskManagement.Application.Utilities.Exceptions;
public class NotFoundException : AppException
{
    public NotFoundException()
        : base(ResultStatus.NotFound, "یافت نشد!") { }
    public NotFoundException(string message)
        : base(ResultStatus.NotFound, message) { }
    public NotFoundException(string message, Exception innerException)
        : base(ResultStatus.NotFound, message, innerException) { }
}
