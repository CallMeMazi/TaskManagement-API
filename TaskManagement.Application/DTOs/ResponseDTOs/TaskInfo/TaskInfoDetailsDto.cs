namespace TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
public record TaskInfoDetailsDto(
    string TaskInfoDescription,
    DateTime StartedTaskAt,
    DateTime EndedTaskAt,
    byte TotalHourse
);