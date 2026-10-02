using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;

namespace TaskManagement.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();

    public GeneralResult<string> Hash(string password)
    {
        var passwordHash = _hasher.HashPassword(new object(), password);

        return GeneralResult<string>.Success(passwordHash);
    }
    public GeneralResult Verify(string hashedPassword, string providedPassword)
    {
        var verifyResult = _hasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword);

        return verifyResult == PasswordVerificationResult.Success 
            ? GeneralResult.Success()
            : GeneralResult.Failure();
    }
    public void VerifyAndCheck(string hashedPassword, string providedPassword, string message)
    {
        if (!Verify(hashedPassword, providedPassword).IsSuccess)
            throw new ValidationFailureException(message);
    }
}
