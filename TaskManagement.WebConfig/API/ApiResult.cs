using Newtonsoft.Json;
using System.Net;
using TaskManagement.Application.Utilities.Enums;
using TaskManagement.Common.Helpers;

namespace TaskManagement.WebConfig.API;
public class ApiResult
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode Status { get; set; }
    public int StatusCode => (int)Status;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? ErrorMessages { get; set; }


    public ApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Status = IsSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        Message = isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        ErrorMessages = null;
    }
    public ApiResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Status = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = null;
    }
    public ApiResult(string message, List<string>? errorMessages)
    {
        IsSuccess = false;
        Status = HttpStatusCode.InternalServerError;
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = errorMessages ?? [Message];
    }
    public ApiResult(string message, ResultStatus? resultStatus)
    {
        IsSuccess = false;
        Status = MapResultStatusToHttpStatusCode(resultStatus);
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = null;
    }
    public ApiResult(string message, List<string>? errorMessages, ResultStatus? resultStatus)
    {
        IsSuccess = false;
        Status = MapResultStatusToHttpStatusCode(resultStatus);
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = errorMessages ?? [message];
    }


    public static ApiResult Success(string message = "عملیات با موفیت انجام شد.")
        => new ApiResult(true, message);
    public static ApiResult Error(string message = "خطایی در سرور رخ داد!", ResultStatus? reslutStatus = null
        , List<string>? errorMessages = null)
            => new ApiResult(message, errorMessages, reslutStatus);

    private static string ValidationMessage(string? message, bool isSuccess)
    {
        if (message.IsNullParameter())
            return isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        else
            return message!;
    }
    private static HttpStatusCode MapResultStatusToHttpStatusCode(ResultStatus? resultStatus) => resultStatus switch
    {
        ResultStatus.OK => HttpStatusCode.OK,
        ResultStatus.ValidaitonFailure => HttpStatusCode.BadRequest,
        ResultStatus.Unauthorized => HttpStatusCode.Unauthorized,
        ResultStatus.Forbidden => HttpStatusCode.Forbidden,
        ResultStatus.NotFound => HttpStatusCode.NotFound,
        ResultStatus.Conflict => HttpStatusCode.Conflict,
        null => HttpStatusCode.InternalServerError,
        _ => HttpStatusCode.InternalServerError
    };
}

public class ApiResult<T> : ApiResult
{
    public T? Result { get; set; }


    public ApiResult(T result)
        : base(true)
        => Result = result;
    public ApiResult(string message, T result)
        : base(true, message)
        => Result = result;
    public ApiResult(string message, ResultStatus resultStatus)
        : base(message, resultStatus) { }


    public static ApiResult<TResult> Success<TResult>(TResult result, string message = "عملیات با موفیت انجام شد.")
        => new ApiResult<TResult>(message, result);
}