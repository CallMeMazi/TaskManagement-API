using AutoMapper;
using TaskManagement.Common.Settings;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface ICommonService
{
    AppSettings AppSettings { get; }
    IMapper Mapper { get; }
    IPasswordService Password { get; }
    IJwtService Jwt { get; }
    IIdGeneratorService IdGenerator { get; }
}
