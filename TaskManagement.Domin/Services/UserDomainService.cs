using System.Net;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Exceptions;
using TaskManagement.Domin.Enums.Roles;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Domin.Interface.Services;

namespace TaskManagement.Domin.Services;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationRepository _orgRepository;
    private readonly IOrganizationMemberShipRepository _orgMemberShipRepository;


    public UserDomainService(IUserRepository userRepository, IOrganizationRepository orgRepository, IOrganizationMemberShipRepository orgMemberShipRepository)
    {
        _userRepository = userRepository;
        _orgRepository = orgRepository;
        _orgMemberShipRepository = orgMemberShipRepository;
    }


    public async Task EnsureCanCreateUserAsync(string mobileNumber, CancellationToken ct)
    {
        if (await _userRepository.IsEntityExistByFilterAsync(u => u.MobileNumber == mobileNumber, ct))
            throw new AppException(HttpStatusCode.BadRequest, ResultStatus.BadRequest, "کاربری با این شماره موبایل وجود دارد!");
    }
    public async Task EnsureCanDeleteUserAsync(int userId, CancellationToken ct)
    {
        if (await _orgRepository.IsEntityExistByFilterAsync(o => o.OwnerId == userId && o.IsActive, ct))
            throw new AppException(
                HttpStatusCode.BadRequest,
                ResultStatus.BadRequest,
                "شما هنوز سازمان فعال دارید، اول سازمان های خود را غیرفعال کنید!"
            );

        var isUserInOtherOrgs = await _orgMemberShipRepository.IsEntityExistByFilterAsync(om =>
            om.UserId == userId
            && om.Role != OrganizationRoles.Owner,
            ct
        );
        if (isUserInOtherOrgs)
            throw new AppException(
                HttpStatusCode.BadRequest,
                ResultStatus.BadRequest,
                "شما در سازمان های دیگری عضو هستید، ابتدا از تمام سازمان هایی که عضو هستید خارج شوید!"
            );
    }
}
