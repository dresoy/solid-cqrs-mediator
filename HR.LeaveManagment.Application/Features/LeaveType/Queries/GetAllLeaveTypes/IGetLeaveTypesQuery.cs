using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes
{
    public interface IGetLeaveTypesQuery : IRequest<List<LeaveTypeDto>>;
}
