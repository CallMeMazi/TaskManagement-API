namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface ICommonService
{
    IPasswordService Password { get; }
    IJwtService Jwt { get; }
}
