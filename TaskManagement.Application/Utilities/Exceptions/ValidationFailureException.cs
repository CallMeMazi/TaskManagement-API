using TaskManagement.Application.Utilities.Enums;

namespace TaskManagement.Application.Utilities.Exceptions;

public class ValidationFailureException : AppException
{
    public IReadOnlyCollection<string>? ErrorMessages { get; }

    public ValidationFailureException()
        : base(ResultStatus.Unauthorized, "شما دسترسی ندارید!") { }
    public ValidationFailureException(string message)
        : base(ResultStatus.Unauthorized, message) { }
    public ValidationFailureException(string message, Exception innerException)
        : base(ResultStatus.Unauthorized, message, innerException) { }
    public ValidationFailureException(string message, IEnumerable<string> errorMessages)
        : base(ResultStatus.Unauthorized, message)
        => ErrorMessages = errorMessages.ToArray();
    public ValidationFailureException(string message, IEnumerable<string> errorMessages, Exception innerException)
        : base(ResultStatus.Unauthorized, message, innerException)
        => ErrorMessages = errorMessages.ToArray();
}
