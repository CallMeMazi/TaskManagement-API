using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.Domain.Entities.BaseEntities;
public class OrganizationInvitation : BaseEntity
{
    public long OrgId { get; private set; }
    public long UserId { get; private set; }
    public string Token { get; private set; }
    public OrgInvitationStatusType Status { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public byte[] RowVersion { get; private set; }

    #region Navigation Prop

    public Organization Org { get; private set; }
    public User User { get; private set; }

    #endregion

    private OrganizationInvitation() { }

    public OrganizationInvitation(long orgId, long userId)
    {
        ValidateOrgInvitation(orgId, userId);

        OrgId = orgId;
        UserId = userId;
        Token = GenerateToken();
        Status = OrgInvitationStatusType.Pending;
        ExpiredAt = DateTime.Now.AddDays(1);
    }


    public void AcceptInvite()
    {
        if (Status != OrgInvitationStatusType.Pending)
            throw new DomainLogicalException("لینک نامعتبر است!");

        Status = OrgInvitationStatusType.Accepted;
        UpdatedAt = DateTime.Now;
    }
    public void ExpiredInvite()
    {
        if (Status != OrgInvitationStatusType.Pending)
            throw new DomainLogicalException("نمیتوانید درخواست دعوت را منقضی کنید!");

        if (ExpiredAt > DateTime.Now)
            throw new DomainLogicalException("زمان انقضای درخواست دعوت هنوز فرا نرسیده است!");

        Status = OrgInvitationStatusType.Expired;
        UpdatedAt = DateTime.Now;
    }
    public void RevokedInvite()
    {
        if (Status != OrgInvitationStatusType.Pending)
            throw new DomainLogicalException("نمیتوانید درخواست دعوت را منقضی کنید!");

        Status = OrgInvitationStatusType.Revoked;
        RevokedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void ValidateOrgInvitation(long orgId, long userId)
    {
        var errorMessages = new List<string>();

        if (orgId <= 0)
            errorMessages.Add("آیدی سازمان خالی است!");

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (errorMessages.Any())
            throw new DomainValidationFailureException("اطلاعات نامعبر هستند!", errorMessages);
    }

    private string GenerateToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}
