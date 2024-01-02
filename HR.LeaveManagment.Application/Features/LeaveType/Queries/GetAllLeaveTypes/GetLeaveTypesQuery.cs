using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes
{
    public interface GetLeaveTypesQuery : IRequest<List<LeaveTypeDto>>;
}
