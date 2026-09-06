using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public record AcceptInvitationCommand(
    long UserId,
    string Token
) : IRequest<GeneralResult>;