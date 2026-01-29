namespace TaskManagement.Common.Classes;
public class GeneralResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    protected GeneralResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static GeneralResult Success(string message = "عملیات با موفقیت انجام شد.") =>
        new GeneralResult(true, message);
    public static GeneralResult Failure(string message = "عملیات ناموفق بود!") =>
        new GeneralResult(false, message);
}

public class GeneralResult<T> : GeneralResult
{
    public T? Result { get; set; }

    private GeneralResult(T? result, bool isSuccess, string message)
        : base(isSuccess, message)
    {
        Result = result;
    }

    public static GeneralResult<T> Success(T result, string message = "عملیات با موفقیت انجام شد.") =>
        new GeneralResult<T>(result, true, message);
    public static GeneralResult<T> Failure(T? result, string message = "عملیات ناموفق بود!") =>
        new GeneralResult<T>(result ?? default, false, message);
}
