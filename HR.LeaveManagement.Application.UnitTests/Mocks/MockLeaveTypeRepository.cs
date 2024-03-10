using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.UnitTests.Mocks.LeaveTypeMocks.Data;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetMockLeaveTypeRepository()
        {
            List<LeaveType> leaveTypes = LeaveTypeMockData.GetList();

            var mockRepo = new Mock<ILeaveTypeRepository>();

            mockRepo.Setup(s => s.GetAsync()).ReturnsAsync(leaveTypes);

            mockRepo.Setup(s => s.CreateAsync(It.IsAny<LeaveType>()))
                .ReturnsAsync((LeaveType leaveType) =>
                {
                    leaveTypes.Add(leaveType);
                    return leaveType;
                });

            SetUpLeaveTypeDetail(mockRepo);

            return mockRepo;
        }

        private static void SetUpLeaveTypeDetail(Mock<ILeaveTypeRepository> mockRepo)
        {
            mockRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => LeaveTypeMockData.GetList().Where(w => w.Id == id).First());
        }

    }
}