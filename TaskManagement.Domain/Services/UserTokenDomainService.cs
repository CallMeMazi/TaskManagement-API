using System.Net;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Exceptions;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Domain.Services;

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
            throw new BadRequestException("نمیتوانید با بیشتر از سه دستگاه یا مرورگر متفاوت وارد شوید!");
    }
}
