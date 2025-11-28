using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class User : BaseEntity
{
    public string MobileNumber { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string SecurityStamp { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public byte Points { get; private set; }
    public DateTime? LastLoginDate { get; private set; }

    #region Navigation Prop

    public ICollection<Organization> OrgAsOwner { get; private set; }
    public ICollection<OrganizationMemberShip> OrgAsMember { get; private set; }
    public ICollection<Project> ProjAsCreator { get; private set; }
    public ICollection<ProjectMemberShip> ProjAsMember { get; private set; }
    public ICollection<Task> TaskAsCreator { get; private set; }
    public ICollection<TaskAssignment> MyTasks { get; private set; }
    public ICollection<TaskInfo> MyTaskInfo { get; private set; }
    public ICollection<UserToken> MyTokens { get; private set; }
    public ICollection<OrganizationInvitation> Invitations { get; private set; }

    #endregion


    private User() { }
    public User(string mobileNumber, string email, string password
        , string firstName, string lastName)
    {
        ValidateUserCreating(mobileNumber, email, password, firstName, lastName);

        MobileNumber = mobileNumber;
        Email = email;
        PasswordHash = password;
        FirstName = firstName;
        LastName = lastName;
        ChangeSecurityStamp();
    }


    public void UpdateUser(string email, string firstName, string lastName)
    {
        ValidateUserUpdating(email, firstName, lastName);

        Email = email;
        FirstName = firstName;
        LastName = lastName;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeUserPassword(string newPassword)
    {
        if (newPassword.IsNullParameter())
            throw new BadRequestException("رمز عبور خالی است!");

        PasswordHash = newPassword;
        ChangeSecurityStamp();
        UpdatedAt = DateTime.Now;
    }
    public void ChangeSecurityStamp()
        =>  SecurityStamp = Guid.NewGuid().ToString();
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

        if (!StringHelper.MobileRegex.IsMatch(mobileNumber))
            errorMessages.Add("شماره موبایل نامعتبر است. باید با 09 شروع شود و 11 رقم باشد.");

        if (email.IsValidEmail())
            errorMessages.Add("ایمیل نامعتبر است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
    public void ValidateUserUpdating(string email, string firstName, string lastName)
    {
        var errorMessages = new List<string>();

        if (email.IsNullParameter())
            errorMessages.Add("ایمیل خالی است!");


        if (firstName.IsNullParameter())
            errorMessages.Add("نام خالی است!");

        if (lastName.IsNullParameter())
            errorMessages.Add("نام خانوادگی خالی است!");

        if (email.IsValidEmail())
            errorMessages.Add("ایمیل نامعتبر است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
}
