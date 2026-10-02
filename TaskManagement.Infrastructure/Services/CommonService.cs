using AutoMapper;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Settings;

namespace TaskManagement.Infrastructure.Services;
public class CommonService : ICommonService
{
    public AppSettings AppSettings { get; private set; }
    public IMapper Mapper { get; private set; }
    public IPasswordService Password { get; private set; }
    public IJwtService Jwt { get; private set; }
    public IIdGeneratorService IdGenerator { get; private set; }

    public CommonService(AppSettings appSettings, IMapper mapper, IPasswordService password
        , IJwtService jwt, IIdGeneratorService idGenerator)
    {
        AppSettings = appSettings;
        Mapper = mapper;
        Password = password;
        Jwt = jwt;
        IdGenerator = idGenerator;
    }
}
