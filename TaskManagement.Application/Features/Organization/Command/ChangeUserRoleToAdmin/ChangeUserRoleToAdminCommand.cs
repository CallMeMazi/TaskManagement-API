using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
public record ChangeUserRoleToAdminCommand(
    long OrgOwnerId,
    long OrgId,
    long UserId
) : IRequest;