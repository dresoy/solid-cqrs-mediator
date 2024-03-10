using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.UnitTests.Mocks.LeaveTypeMocks.Data
{
    public static class LeaveTypeMockData
    {
        public static List<LeaveType> GetList() => [
                new LeaveType
                {
                    Id = 1, DefaultDays = 10,
                    Name = "Test Vacation"
                },
                new LeaveType
                {
                    Id = 2, DefaultDays = 15,
                    Name="Test Sick"
                },
                new LeaveType
                {
                    Id = 3,
                    DefaultDays = 15,
                    Name = "Test Maternity"
                }
            ];
    }
}
