using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IPasswordService
{
    GeneralResult<string> Hash(string password);
    GeneralResult Verify(string hashedPassword, string providedPassword);
    void VerifyAndCheck(string hashedPassword, string providedPassword, string message);
}
