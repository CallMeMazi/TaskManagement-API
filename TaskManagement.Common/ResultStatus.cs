namespace TaskManagement.Common;
public enum ResultStatus
{
    Success = 200,
    BadRequest = 400,
    UnAuthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    ServerError = 500
}
