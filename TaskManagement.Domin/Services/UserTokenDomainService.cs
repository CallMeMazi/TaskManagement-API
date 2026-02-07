using System.Net;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Exceptions;
using TaskManagement.Domin.Enums.Statuses;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Domin.Interface.Services;

namespace TaskManagement.Domin.Services;

public class UserTokenDomainService : IUserTokenDomainService
{
    private readonly IUserTokenRepository _tokenRepository;


    public UserTokenDomainService(IUserTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }


    public async Task EnsureCanLoginAsync(int userId, CancellationToken ct)
    {
        var activeDevice = await _tokenRepository.GetCountByFilterAsync(ut =>
            ut.UserId == userId
            && ut.TokenStatus == TokenStatus.Active,
            ct
        );
        if (activeDevice >= 3)
            throw new AppException(
                HttpStatusCode.BadRequest,
                ResultStatus.BadRequest,
                "نمیتوانید با بیشتر از سه دستگاه یا مرورگر متفاوت وارد شوید!"
            );
    }
}
