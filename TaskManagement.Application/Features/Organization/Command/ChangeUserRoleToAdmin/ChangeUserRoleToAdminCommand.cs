using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
public record ChangeUserRoleToAdminCommand(
    long OrgOwnerId,
    long OrgId,
    long UserId
) : IRequest<GeneralResult>;