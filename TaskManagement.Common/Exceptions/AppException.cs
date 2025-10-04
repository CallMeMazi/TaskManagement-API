using System.Net;
using TaskManagement.Common.Enums;

namespace TaskManagement.Common.Exceptions;
public class AppException : Exception
{
    public HttpStatusCode HttpStatus { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public List<string>? ErrorMessages { get; set; }


    public AppException() 
       : this(HttpStatusCode.InternalServerError)
    {
    }

    public AppException(HttpStatusCode httpStatus) 
       : this(httpStatus, ResultStatus.ServerError)
    {
    }

    public AppException(HttpStatusCode httpStatus, ResultStatus resultStatus)
       : this(httpStatus, resultStatus, "خطایی در سرور رخ داد!")
    {
    }

    public AppException(HttpStatusCode httpStatus, ResultStatus resultStatus, string? message)
       : this(httpStatus, resultStatus, message, null)
    {
    }

    public AppException(HttpStatusCode httpStatus, ResultStatus resultStatus, string? message, List<string>? errorMessages)
       : this(httpStatus, resultStatus, message, errorMessages, null)
    {
    }

    public AppException(HttpStatusCode httpStatus, ResultStatus resultStatus, string? message, List<string>? errorMessages, Exception? innerException)
       : base(message, innerException)
    {
        HttpStatus = httpStatus;
        ResultStatus = resultStatus;
        ErrorMessages = errorMessages ?? new() { "خطایی در سرور رخ داد!"};
    }

}
