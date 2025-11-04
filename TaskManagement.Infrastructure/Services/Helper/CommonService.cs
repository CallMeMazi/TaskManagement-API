using TaskManagement.Application.Interfaces.Services.Halper;

namespace TaskManagement.Infrastructure.Services.Helper;
public class CommonService : ICommonService
{
    public IPasswordService Password { get; set; }
    public IJwtService Jwt { get; set; }


    public CommonService(IPasswordService password, IJwtService jwt)
    {
        Password = password;
        Jwt = jwt;
    }
}
