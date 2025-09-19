using Newtonsoft.Json;
using TaskManagement.Common;
using TaskManagement.Common.Helpers;

namespace TaskManagement.WebConfig.API;
public class ApiResult
{
    public bool IsSuccess { get; set; }
    public ResultStatus ResultStatus { get; set; }
    public int StatusCode => (int)ResultStatus;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public required string Message { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string[]? ErrorMessages { get; set; }


    public ApiResult()
    {
        IsSuccess = false;
        Message = "خطایی در سرور رخ داد!";
        ResultStatus = ResultStatus.ServerError;
        ErrorMessages = new[] { "خطایی در سرور رخ داد!" };
    }

    public ApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        ResultStatus = IsSuccess ? ResultStatus.Success : ResultStatus.ServerError;
        Message = isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        ErrorMessages = isSuccess ? null : new[] { "خطایی در سرور رخ داد!" };
    }

    public ApiResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        ResultStatus = isSuccess ? ResultStatus.Success : ResultStatus.ServerError;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = isSuccess ? null : new[] { "خطایی در سرور رخ داد!" };
    }

    public ApiResult(bool isSuccess, string message, string[]? errorMessages)
    {
        IsSuccess = isSuccess;
        ResultStatus = isSuccess ? ResultStatus.Success : ResultStatus.ServerError; ;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = errorMessages ?? new[] { "خطایی در سرور رخ داد!" };
    }

    public ApiResult(bool isSuccess, string message, ResultStatus resultStatus)
    {
        IsSuccess = isSuccess;
        ResultStatus = resultStatus;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = isSuccess ? null : new[] { "خطایی در سرور رخ داد!" };
    }

    public ApiResult(bool isSuccess, string message, string[]? errorMessages, ResultStatus resultStatus)
    {
        IsSuccess = isSuccess;
        ResultStatus = resultStatus;
        Message = ValidationMessage(message, isSuccess);
        ErrorMessages = errorMessages ?? new[] { "خطایی در سرور رخ داد!" };
    }


    private static string ValidationMessage(string message, bool isSuccess)
    {
        if (message.IsNullParameter())
            return isSuccess ? "عملیات با موفیت انجام شد." : "خطایی در سرور رخ داد!";
        else
            return message;
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
}