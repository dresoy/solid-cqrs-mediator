using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail
{
    public record GetLeaveRequestDetailsQuery(int Id) : IRequest<LeaveRequestDetailsDto>;
}
