using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocations
{
    public class LeaveAllocationDto
    {
        public int Id { get; set; }

        public int NumberOfDays { get; set; }

        public LeaveTypeDto LeaveType { get; set; }

        public int LeavyTypeId { get; set; }

        public int Period { get; set; }
    }
}
