using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Domain.Interface.Services;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.Domain.Services;

public class UserTokenDomainService : IUserTokenDomainService
{
    private readonly IUserTokenRepository _tokenRepository;


    public UserTokenDomainService(IUserTokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }


    public async Task EnsureCanLoginAsync(long userId, CancellationToken ct)
    {
        var activeDevice = await _tokenRepository.GetCountByFilterAsync(ut =>
            ut.UserId == userId
            && ut.TokenStatus == TokenStatusType.Active,
            ct
        );
        if (activeDevice >= 3)
            throw new DomainLogicalException("نمیتوانید با بیشتر از سه دستگاه یا مرورگر متفاوت وارد شوید!");
    }
}
