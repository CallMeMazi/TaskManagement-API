using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.DTOs.ResponseDTOs.Project;
public record ProjectDetailsDto(
    string ProjName,
    string projDescription,
    byte ProjProgress,
    ProjectStatusType ProjStatus,
    bool IsActive,
    DateTime ProjStartAt,
    DateTime ProjEndAt,
    byte ProjMaxUsers,
    byte ProjMaxTasks,
    DateTime CreateAt
);