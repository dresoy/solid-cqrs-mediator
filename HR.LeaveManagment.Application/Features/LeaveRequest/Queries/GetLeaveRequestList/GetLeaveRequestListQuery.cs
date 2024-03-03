using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList
{
    public record GetLeaveRequestListQuery : IRequest<List<LeaveRequestListDto>>;
}
