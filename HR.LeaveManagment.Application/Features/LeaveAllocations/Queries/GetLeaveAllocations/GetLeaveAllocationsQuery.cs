using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocations
{
    public class GetLeaveAllocationsQuery : IRequest<List<LeaveAllocationDto>>
    {
    }
}
