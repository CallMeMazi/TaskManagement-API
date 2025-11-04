namespace TaskManagement.Application.Interfaces.Services.Halper;
public interface IPasswordService
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}
