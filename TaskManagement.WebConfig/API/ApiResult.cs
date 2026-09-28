using Newtonsoft.Json;
using System.Net;
using TaskManagement.Application.Utilities.Enums;
using TaskManagement.Common.Helpers;

namespace TaskManagement.WebConfig.API;
public class ApiResult
{
    public bool IsSuccess { get; set; }
    public ResultStatus Status { get; set; }
    public int StatusCode => (int)Status;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? ErrorMessages { get; set; }


    public ApiResult()
    {
        IsSuccess = false;
        Message = "خطایی در سرور رخ داد!";
        Status = ResultStatus.ServerError;
        ErrorMessages = [Message];
    }
    public ApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Status = IsSuccess ? ResultStatus.OK : ResultStatus.ServerError;
        Message = isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        ErrorMessages = isSuccess ? null : [Message];
    }
    public ApiResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Status = isSuccess ? ResultStatus.OK : ResultStatus.ServerError;
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = isSuccess ? null : [Message];
    }
    public ApiResult(string message, List<string>? errorMessages)
    {
        IsSuccess = false;
        Status = ResultStatus.ServerError;
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = errorMessages ?? [Message];
    }
    public ApiResult(string message, ResultStatus resultStatus)
    {
        IsSuccess = false;
        Status = resultStatus;
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = [Message];
    }
    public ApiResult(string message, List<string>? errorMessages, ResultStatus resultStatus)
    {
        IsSuccess = false;
        Status = resultStatus;
        Message = ValidationMessage(message, IsSuccess);
        ErrorMessages = errorMessages ?? [message];
    }


    public static ApiResult Success(string message = "عملیات با موفیت انجام شد.")
        => new ApiResult(true, message, ResultStatus.OK);
    public static ApiResult Error(string message = "خطایی در سرور رخ داد!", ResultStatus status = ResultStatus.ServerError
        , List<string>? errorMessages = null)
            => new ApiResult(message, errorMessages, status);

    private static string ValidationMessage(string? message, bool isSuccess)
    {
        if (message.IsNullParameter())
            return isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        else
            return message!;
    }
}

public class ApiResult<T> : ApiResult
{
    public T? Result { get; set; }


    public ApiResult()
        : base(true)
    {
    }
    public ApiResult(T result)
        : base(true)
    {
        Result = result;
    }
    public ApiResult(string message, T result)
        : base(true, message)
    {
        Result = result;
    }
    public ApiResult(string message, HttpStatusCode resultStatus, T result)
        : base(true, message, resultStatus)
    {
        Result = result;
    }


    public static ApiResult<TResult> Success<TResult>(TResult result, string message = "عملیات با موفیت انجام شد.")
        => new ApiResult<TResult>(message, HttpStatusCode.OK, result);
}