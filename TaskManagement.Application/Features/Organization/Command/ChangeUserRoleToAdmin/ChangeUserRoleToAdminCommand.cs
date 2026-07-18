using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
public record ChangeUserRoleToAdminCommand(
    int OrgOwnerId,
    int OrgId,
    int UserId
) : IRequest<GeneralResult>;