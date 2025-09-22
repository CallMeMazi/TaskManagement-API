using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class Organization : BaseEntity
{
    public string OrgName { get; private set; }
    public Guid OwnerId { get; private set; }
    public string OrgCode { get; private set; }
    public string OrgDescription { get; private set; }
    public bool IsActive { get; private set; } = true;
    public byte MaxUsers { get; private set; }

    public User Owner { get; private set; }
    public ICollection<OrganizationMemberShip> Members { get; private set; }
    public ICollection<Project> Projects { get; private set; }


    private Organization() { }
    public Organization(string orgName, Guid ownerId, string orgDescription
        , byte maxUsers)
    {
        ValidateOrgCreating(orgName, ownerId, orgDescription, maxUsers);

        OrgName = orgName;
        OwnerId = ownerId;
        OrgDescription = orgDescription;
        MaxUsers = maxUsers;
        OrgCode = GenerateOrgCode(orgName);
    }


    public void UpdateOrg(string orgName, string orgDescription)
    {
        ValidateOrgUpdating(orgName, orgDescription);

        OrgName = orgName;
        OrgDescription = orgDescription;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeOrgActivity(bool activity)
    {
        if (IsActive == activity)
            throw new BadRequestException(IsActive ? "سازمان در حال حاضر فعال است!" : "سازمان در حال حاضر غیر فعال است!");

        IsActive = activity;

        UpdatedAt = DateTime.Now;
    }

    public void ValidateOrgCreating(string orgName, Guid ownerId, string orgDescription
        , int maxUser)
    {
        var errorMessages = new List<string>();

        if (orgName.IsNullParameter())
            errorMessages.Add("نام سازمان خالی است!");

        if (ownerId == Guid.Empty)
            errorMessages.Add("آیدی سازنده سازمان نامعتبر است!");

        if (orgDescription.IsNullParameter())
            errorMessages.Add("توضیحات سازمان خالی است!");

        if (maxUser <= 5)
            errorMessages.Add("حداقل افراد سازمان باید بیشتر از 5 نفر باشد!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
    public void ValidateOrgUpdating(string orgName, string orgDescription)
    {
        var errorMessages = new List<string>();

        if (orgName.IsNullParameter())
            errorMessages.Add("نام سازمان خالی است!");

        if (orgDescription.IsNullParameter())
            errorMessages.Add("توضیحات سازمان خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }

    private string GenerateOrgCode(string orgName)
        => $"ORG-{orgName}-{DateTime.Now:yyyyMMddHHmmss}";
}
