using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public record UpdateLeaveTypeCommand(string Name, int DefaultDays) : IRequest<Unit>;
}
