namespace TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
public record TaskInfoDetailsDto(
    string TaskInfoDescription,
    DateTime StartedTaskAt,
    DateTime EndedTaskAt,
    byte TotalHourse
);