using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class UserToken : IBaseEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public Guid SecurityStamp { get; private set; }
    public TokenStatus TokenStatus { get; private set; } = TokenStatus.Active;
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime ExpiredAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime LastUsedAt { get; private set; } = DateTime.Now;
    public string UserIp { get; private set; }

    public User User { get; private set; }


    private UserToken() { }
    public UserToken(Guid userId, string token, Guid securityStamp
        , DateTime expiredAt, string userIp)
    {
        ValidateUserToken(userId, token, securityStamp, expiredAt, userIp);

        UserId = userId;
        Token = token;
        SecurityStamp = securityStamp;
        ExpiredAt = expiredAt;
        UserIp = userIp;
    }


    public void ChangeTokenSecurityStamp()
        => SecurityStamp = Guid.NewGuid();
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
    public void UseToken()
    {
        LastUsedAt = DateTime.Now;
    }

    public void ValidateUserToken(Guid userId, string token, Guid securityStamp
        , DateTime expiredAt, string userIp)
    {
        var errorMessages = new List<string>();

        if (userId == Guid.Empty)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (securityStamp == Guid.Empty)
            errorMessages.Add("مهر امنیتی خالی است!");

        if (token.IsNullParameter())
            errorMessages.Add("توکن خالی است!");

        if (expiredAt <= DateTime.Now)
            errorMessages.Add("تاریخ انقضا نامعتبر است!");

        if (userIp.IsNullParameter())
            errorMessages.Add("آیپی کابر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
