using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class UserToken : BaseEntity
{
    public int UserId { get; private set; }
    public string AccessTokenHash { get; private set; }
    public string RefreshTokenHash { get; private set; }
    public string SecurityStamp { get; set; }
    public TokenStatus TokenStatus { get; private set; } = TokenStatus.Active;
    public DateTime ExpiredAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime LastUsedAt { get; private set; } = DateTime.Now;
    public string DeviceId { get; private set; }
    public string UserIp { get; private set; }
    public string UserAgent { get; private set; }

    #region Navigation Prop

    public User User { get; private set; }

    #endregion


    private UserToken() { }
    public UserToken(int userId, string accessToken, string refreshToken
        , string securityStamp, DateTime expiredAt, string deviceId
        , string userIp, string userAgent)
    {
        ValidateUserToken(userId, accessToken, refreshToken, securityStamp, expiredAt, deviceId, userIp, userAgent);

        UserId = userId;
        AccessTokenHash = accessToken;
        RefreshTokenHash = refreshToken;
        SecurityStamp = securityStamp;
        ExpiredAt = expiredAt;
        DeviceId = deviceId;
        UserIp = userIp;
        UserAgent = userAgent;
    }

    
    public void RefreshToken(string accessToken, string refreshToken, int ExpirationRefreshToken)
    {
        if (accessToken.IsNullParameter())
            throw new BadRequestException("توکن خالی است!");

        if (refreshToken.IsNullParameter())
            throw new BadRequestException("رفرش توکن خالی است!");

        AccessTokenHash = accessToken;
        RefreshTokenHash = refreshToken;
        ExpiredAt = DateTime.Now.AddDays(ExpirationRefreshToken);
        LastUsedAt = DateTime.Now;
    }
    public void RevokeToken()
    {
        if (TokenStatus == TokenStatus.Revoked || TokenStatus == TokenStatus.Expired)
            return;

        TokenStatus = TokenStatus.Revoked;
        RevokedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    public void ExpiredToken()
    {
        if (ExpiredAt > DateTime.Now)
            throw new BadRequestException("توکن هنوز مهلت دارد!");

        if (TokenStatus == TokenStatus.Revoked || TokenStatus == TokenStatus.Expired)
            return;

        TokenStatus = TokenStatus.Expired;
        UpdatedAt = DateTime.Now;
    }

    public void ValidateUserToken(int userId, string accessToken, string refreshToken
        , string securityStamp, DateTime expiredAt, string deviceId
        , string userIp, string userAgent)
    {
        var errorMessages = new List<string>();

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (accessToken.IsNullParameter())
            errorMessages.Add("توکن خالی است!");

        if (refreshToken.IsNullParameter())
            errorMessages.Add("رفرش توکن خالی است!");

        if (securityStamp.IsNullParameter())
            errorMessages.Add("مهر امنیتی خالی است!");

        if (expiredAt <= DateTime.Now)
            errorMessages.Add("تاریخ انقضا نامعتبر است!");

        if (deviceId.IsNullParameter())
            throw new Exception($"deviceId is null, error in {nameof(ValidateUserToken)} method!");

        if (userIp.IsNullParameter())
            errorMessages.Add("آیپی کابر خالی است!");

        if (userAgent.IsNullParameter())
            errorMessages.Add("اطلاعات دستگاه کابر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
