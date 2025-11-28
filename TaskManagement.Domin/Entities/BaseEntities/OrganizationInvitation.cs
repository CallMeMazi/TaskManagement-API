using TaskManagement.Common.Exceptions;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class OrganizationInvitation : BaseEntity
{
    public int OrgId { get; private set; }
    public int UserId { get; private set; }
    public string Token { get; private set; }
    public OrgInvitationStatus Status { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public byte[] RowVersion { get; private set; }

    #region Navigation Prop

    public Organization Org { get; private set; }
    public User User { get; private set; }

    #endregion

    private OrganizationInvitation() { }

    public OrganizationInvitation(int orgId, int userId)
    {
        ValidateOrgInvitation(orgId, userId);

        OrgId = orgId;
        UserId = userId;
        Token = GenerateToken();
        Status = OrgInvitationStatus.Pending;
        ExpiredAt = DateTime.Now.AddDays(1);
    }


    public void AcceptInvite()
    {
        if (Status != OrgInvitationStatus.Pending)
            throw new BadRequestException("لینک نامعتبر است!");

        Status = OrgInvitationStatus.Accepted;
        UpdatedAt = DateTime.Now;
    }
    public void ExpiredInvite()
    {
        if (Status != OrgInvitationStatus.Pending)
            throw new BadRequestException("نمیتوانید درخواست دعوت را منقضی کنید!");

        if (ExpiredAt > DateTime.Now)
            throw new BadRequestException("زمان انقضای درخواست دعوت هنوز فرا نرسیده است!");

        Status = OrgInvitationStatus.Expired;
        UpdatedAt = DateTime.Now;
    }
    public void RevokedInvite()
    {
        if (Status != OrgInvitationStatus.Pending)
            throw new BadRequestException("نمیتوانید درخواست دعوت را منقضی کنید!");

        Status = OrgInvitationStatus.Revoked;
        RevokedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void ValidateOrgInvitation(int orgId, int userId)
    {
        var errorMessages = new List<string>();

        if (orgId <= 0)
            errorMessages.Add("آیدی سازمان خالی است!");

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }

    private string GenerateToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}
