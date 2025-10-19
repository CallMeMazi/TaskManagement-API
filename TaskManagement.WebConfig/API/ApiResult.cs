using Newtonsoft.Json;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Helpers;

namespace TaskManagement.WebConfig.API;
public class ApiResult
{
    public bool IsSuccess { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public int StatusCode => (int)ResultStatus;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? ErrorMessages { get; set; }


    public ApiResult()
    {
        IsSuccess = false;
        Message = "خطایی در سرور رخ داد!";
        ResultStatus = ResultStatus.ServerError;
        ErrorMessages = new() { "خطایی در سرور رخ داد!" };
    }
    public ApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        ResultStatus = IsSuccess ? ResultStatus.Success : ResultStatus.ServerError;
        Message = isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        ErrorMessages = isSuccess ? null : new() { "خطایی در سرور رخ داد!" };
    }
    public ApiResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        ResultStatus = isSuccess ? ResultStatus.Success : ResultStatus.ServerError;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = isSuccess ? null : new() { "خطایی در سرور رخ داد!" };
    }
    public ApiResult(bool isSuccess, string message, List<string>? errorMessages)
    {
        IsSuccess = isSuccess;
        ResultStatus = isSuccess ? ResultStatus.Success : ResultStatus.ServerError; ;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = errorMessages ?? new() { message };
    }
    public ApiResult(bool isSuccess, string message, ResultStatus resultStatus)
    {
        IsSuccess = isSuccess;
        ResultStatus = resultStatus;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = isSuccess ? null : new() { "خطایی در سرور رخ داد!" };
    }
    public ApiResult(bool isSuccess, string message, List<string>? errorMessages, ResultStatus resultStatus)
    {
        IsSuccess = isSuccess;
        ResultStatus = resultStatus;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = errorMessages ?? new() { message };
    }


    public static ApiResult Success(string message = "عملیات با موفیت انجام شد.")
        => new ApiResult(true, message, ResultStatus.Success);
    public static ApiResult Error(string message = "خطایی در سرور رخ داد!"
        , ResultStatus status = ResultStatus.ServerError, List<string>? errorMessages = null)
            => new ApiResult(false, message, errorMessages, status);

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
    public ApiResult(string message, ResultStatus resultStatus, T result)
        : base(true, message, resultStatus)
    {
        Result = result;
    }


    public static ApiResult<TResult> Success<TResult>(TResult result, string message = "عملیات با موفیت انجام شد.")
        => new ApiResult<TResult>(message, ResultStatus.Success, result);
}