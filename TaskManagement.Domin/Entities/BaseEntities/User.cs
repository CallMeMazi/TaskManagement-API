using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class User : BaseEntity
{
    public string MobileNumber { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public byte Points { get; private set; }
    public DateTime? LastLoginDate { get; private set; }

    public ICollection<Organization> OrgAsOwner { get; private set; }
    public ICollection<OrganizationMemberShip> OrgAsMember { get; private set; }
    public ICollection<Project> ProjAsCreator { get; private set; }
    public ICollection<ProjectMemberShip> ProjAsMember { get; private set; }
    public ICollection<Task> TaskAsCreator { get; private set; }
    public ICollection<TaskAssignment> MyTasks { get; private set; }
    public ICollection<TaskInfo> MyTaskInfo { get; private set; }
    public ICollection<UserToken> MyTokens { get; private set; }


    private User() { }
    public User(string mobileNumber, string email, string password
        , string firstName, string lastName)
    {
        ValidateUserCreating(mobileNumber, email, password, firstName, lastName);

        MobileNumber = mobileNumber;
        Email = email;
        PasswordHash = password.GetSha256Hash();
        FirstName = firstName;
        LastName = lastName;
    }


    public void UpdateUser(string mobileNumber, string email, string firstName
        , string lastName)
    {
        ValidateUserUpdating(mobileNumber, email, firstName, lastName);

        MobileNumber = mobileNumber;
        Email = email;
        FirstName = firstName;
        LastName = lastName;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeUserPassword(string oldPassword, string newPassword)
    {
        if (oldPassword.IsNullParameter() || newPassword.IsNullParameter())
            throw new BadRequestException("رمز خالی است!");

        if (oldPassword != PasswordHash)
            throw new BadRequestException("رمز نامعتبر است!");

        PasswordHash = newPassword;
        UpdatedAt = DateTime.Now;
    }
    public void ChangeLastLoginDate()
        => LastLoginDate = DateTime.Now;
    public void IncreaseOrDecreasePoints(byte amount)
        => Points += amount;

    public void ValidateUserCreating(string mobileNumber, string email, string password
        , string firstName, string lastName)
    {
        var errorMessages = new List<string>();

        if (mobileNumber.IsNullParameter())
            errorMessages.Add("شماره موبایل خالی است!");

        if (email.IsNullParameter())
            errorMessages.Add("ایمیل خالی است!");

        if (password.IsNullParameter())
            errorMessages.Add("رمز عبور خالی است!");

        if (firstName.IsNullParameter())
            errorMessages.Add("نام خالی است!");

        if (lastName.IsNullParameter())
            errorMessages.Add("نام خانوادگی خالی است!");

        ValidateMobileNumberAndEmail(mobileNumber, email, errorMessages);
    }
    public void ValidateUserUpdating(string mobileNumber, string email, string firstName
        , string lastName)
    {
        var errorMessages = new List<string>();

        if (mobileNumber.IsNullParameter())
            errorMessages.Add("شماره موبایل خالی است!");

        if (email.IsNullParameter())
            errorMessages.Add("ایمیل خالی است!");


        if (firstName.IsNullParameter())
            errorMessages.Add("نام خالی است!");

        if (lastName.IsNullParameter())
            errorMessages.Add("نام خانوادگی خالی است!");

        ValidateMobileNumberAndEmail(mobileNumber, email, errorMessages);
    }
    public void ValidateMobileNumberAndEmail(string mobileNumber, string email, List<string> errorMessages)
    {
        if (!StringHelper.MobileRegex.IsMatch(mobileNumber))
            errorMessages.Add("شماره موبایل نامعتبر است. باید با 09 شروع شود و 11 رقم باشد.");

        if (!StringHelper.EmailRegex.IsMatch(email))
            errorMessages.Add("ایمیل نامعتبر است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
}
