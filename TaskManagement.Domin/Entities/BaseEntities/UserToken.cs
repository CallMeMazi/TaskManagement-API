using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class UserToken : IBaseEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string AccessTokenHash { get; private set; }
    public string RefreshTokenHash { get; private set; }
    public string SecurityStamp { get; set; }
    public TokenStatus TokenStatus { get; private set; } = TokenStatus.Active;
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime ExpiredAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime LastUsedAt { get; private set; } = DateTime.Now;
    public string UserIp { get; private set; }
    public string UserAgent { get; private set; }

    public User User { get; private set; }


    private UserToken() { }
    public UserToken(int userId, string accessToken, string refreshToken
        , string securityStamp, DateTime expiredAt, string userIp
        , string userAgent)
    {
        ValidateUserToken(userId, accessToken, refreshToken, securityStamp, expiredAt, userIp, userAgent);

        UserId = userId;
        AccessTokenHash = accessToken;
        RefreshTokenHash = refreshToken;
        SecurityStamp = securityStamp;
        ExpiredAt = expiredAt;
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
    }
    public void ExpiredToken()
    {
        if (ExpiredAt > DateTime.Now)
            throw new BadRequestException("توکن هنوز مهلت دارد!");

        if (TokenStatus == TokenStatus.Revoked || TokenStatus == TokenStatus.Expired)
            return;

        TokenStatus = TokenStatus.Expired;
    }

    public void ValidateUserToken(int userId, string accessToken, string refreshToken
        , string securityStamp, DateTime expiredAt, string userIp
        , string userAgent)
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

        if (userIp.IsNullParameter())
            errorMessages.Add("آیپی کابر خالی است!");

        if (userAgent.IsNullParameter())
            errorMessages.Add("اطلاعات دستگاه کابر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
