using TaskManagement.Application.Utilities.Enums;

namespace TaskManagement.Application.Utilities.Exceptions;
public abstract class AppException : Exception
{
    public ResultStatus Status { get; }

    public AppException(ResultStatus status, string message, Exception? innerException = null)
        : base(message, innerException)
        => Status = status;
}
