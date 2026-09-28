using TaskManagement.Application.Interfaces.Services.Halper;

namespace TaskManagement.Infrastructure.Services;
public class CommonService : ICommonService
{
    public IPasswordService Password { get; set; }
    public IJwtService Jwt { get; set; }
    public IIdGeneratorService IdGenerator { get; set; }

    public CommonService(IPasswordService password, IJwtService jwt, IIdGeneratorService idGenerator)
    {
        Password = password;
        Jwt = jwt;
        IdGenerator = idGenerator;
    }
}
