using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Exceptions;

namespace TaskManagement.Infrastructure.Utilities;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();


    public string Hash(string password)
    {
        return _hasher.HashPassword(new object(), password);
    }

    public bool Verify(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }

    public void VerifyAndCheck(string hashedPassword, string providedPassword, string message)
    {
        if (!Verify(hashedPassword, providedPassword))
            throw new BadRequestException(message);
    }
}
