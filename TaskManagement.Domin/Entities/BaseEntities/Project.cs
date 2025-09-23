using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class Project : BaseEntity
{
    public string ProjName { get; private set; }
    public string ProjDescription { get; private set; }
    public Guid OrgId { get; private set; }
    public Guid CreatorId { get; private set; }
    public byte ProjProgress { get; private set; }
    public ProjectStatusType ProjStatus { get; private set; } = ProjectStatusType.InProgress;
    public bool IsActive { get; private set; } = true;
    public DateTime ProjStartAt { get; private set; } = DateTime.Now;
    public DateTime? ProjEndAt { get; private set; }
    public byte ProjMaxUsers { get; private set; }
    public byte ProjMaxTasks { get; private set; }

    public Organization Org { get; private set; }
    public User Creator { get; private set; }
    public ICollection<ProjectMemberShip> ProjMember { get; private set; }
    public ICollection<Task> Tasks { get; private set; }


    private Project() { }
    public Project(string projName, string projDescription, Guid orgId
        , Guid creatorId, byte maxUsers, byte maxTasks)
    {
        ValidateProjectCreating(projName, projDescription, orgId, creatorId, maxUsers, maxTasks);

        ProjName = projName;
        ProjDescription = projDescription;
        OrgId = orgId;
        CreatorId = creatorId;
        ProjMaxUsers = maxUsers;
        ProjMaxTasks = maxTasks;
    }


    public void UpdateProject(string projName, string projDescription)
    {
        ValidateProjectUpdating(projName, projDescription);

        ProjName = projName;
        ProjDescription = projDescription;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeProjActivity(bool activity)
    {
        if (ProjStatus == ProjectStatusType.Cancel)
            throw new BadRequestException("پروژه کنسل شده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (ProjStatus == ProjectStatusType.Finished)
            throw new BadRequestException("پروژه به پایان رسیده، نمیتوانید وضعیت آن را تغییر دهید!");

        if (IsActive == activity)
            throw new BadRequestException(IsActive ? "پروژه در حال حاضر فعال است!" : "پروژه در حال حاضر غیر فعال است!");

        IsActive = activity;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeProjStatusToInProgress()
    {
        if (ProjStatus == ProjectStatusType.InProgress)
            return;

        if (ProjStatus == ProjectStatusType.Cancel)
            throw new BadRequestException("پروژه کنسل شده است، نمیتوانید وضعیتت آن را تغییر دهید!");

        if (ProjStatus == ProjectStatusType.Finished)
            throw new BadRequestException("پروژه به اتمام رسیده، نمیتوانید وضعیتت آن را تغییر دهید!");

        ProjStatus = ProjectStatusType.InProgress;
        IsActive = true;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeProjStatusToAdjournment()
    {
        if (ProjStatus == ProjectStatusType.Adjournment)
            return;

        if (ProjStatus == ProjectStatusType.Cancel)
            throw new BadRequestException("پروژه کنسل شده است، نمیتوانید وضعیتت آن را تغییر دهید!");

        if (ProjStatus == ProjectStatusType.Finished)
            throw new BadRequestException("پروژه به اتمام رسیده، نمیتوانید وضعیتت آن را تغییر دهید!");

        ProjStatus = ProjectStatusType.Adjournment;
        IsActive = false;

        UpdatedAt = DateTime.Now;

    }
    public void CancelProj()
    {
        if (ProjStatus == ProjectStatusType.Cancel)
            return;

        if (ProjStatus == ProjectStatusType.Finished)
            throw new BadRequestException("پروژه به اتمام رسیده، نمیتوانید وضعیتت آن را تغییر دهید!");

        ProjStatus = ProjectStatusType.Cancel;
        ProjEndAt = DateTime.Now;
        IsActive = false;

        UpdatedAt = DateTime.Now;
    }
    public void FinishProj()
    {
        if (ProjStatus == ProjectStatusType.Finished)
            return;

        if (ProjStatus == ProjectStatusType.Cancel)
            throw new BadRequestException("پروژه کنسل شده است، نمیتوانید وضعیتت آن را تغییر دهید!");

        ProjStatus = ProjectStatusType.Finished;
        ProjProgress = 100;
        ProjEndAt = DateTime.Now;
        IsActive = false;

        UpdatedAt = DateTime.Now;
    }
    public void ChangeProjProgress(byte progress)
    {
        if (!IsActive)
            throw new BadRequestException("پروژه فعال نیست، نمیتوانید میزان پیشرفت را تغییر دهید!");

        if (ProjProgress == progress)
            return;

        if (progress <= 0 || progress > 100)
            throw new BadRequestException("نمیتوانید میزان پیشرفت را کمتر از 0 و بیشتر از 100 وارد کنید!");

        if (ProjProgress > progress)
            throw new BadRequestException("نمیتوانید میزان پیشرفت را کمتر از میزان فعلی اعمال کنید!");

        ProjProgress = progress;

        if (progress == 100)
        {
            IsActive = false;
            FinishProj();
        }

        ProjEndAt = DateTime.Now;
    }

    public void ValidateProjectCreating(string projName, string projDescription, Guid orgId
        , Guid creatorId, byte maxUsers, byte maxTasks)
    {
        var errorMessages = new List<string>();

        if (projName.IsNullParameter())
            errorMessages.Add("نام پروژه خالی است!");

        if (projDescription.IsNullParameter())
            errorMessages.Add("توضیحات پروژه خالی است!");

        if (orgId == Guid.Empty)
            errorMessages.Add("آیدی سازمان نامعتبر است!");

        if (creatorId == Guid.Empty)
            errorMessages.Add("آیدی کاربر سازنده خالی است!");

        if (maxUsers <= 3)
            errorMessages.Add("حداقل افراد پروژه باید بیشتر از 3 نفر باشد!");

        if (maxTasks <= 3)
            errorMessages.Add("حداقل تسک های پروژه باید بیشتر از 3 تسک باشد!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
    public void ValidateProjectUpdating(string projName, string projDescription)
    {
        var errorMessages = new List<string>();

        if (projName.IsNullParameter())
            errorMessages.Add("نام پروژه خالی است!");

        if (projDescription.IsNullParameter())
            errorMessages.Add("توضیحات پروژه خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعتبر است!", errorMessages);
    }
}
