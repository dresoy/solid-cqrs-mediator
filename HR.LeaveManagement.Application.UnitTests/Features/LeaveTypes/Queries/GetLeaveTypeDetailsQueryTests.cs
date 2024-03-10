using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using HR.LeaveManagement.Application.UnitTests.Mocks.LeaveTypeMocks.Data;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeDetailsQueryTests
    {
        private Mock<ILeaveTypeRepository> _mockRepo;
        private IMapper _mapper;
        private Mock<IAppLogger<GetLeaveTypeDetailsHandler>> _mockAppLogger;

        public GetLeaveTypeDetailsQueryTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(s =>
            {
                s.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _mockAppLogger = new Mock<IAppLogger<GetLeaveTypeDetailsHandler>>();
        }

        [Fact]
        public async Task GetLeaveTypeDetailTests()
        {
            LeaveTypeDetailsDto expected = _mapper.Map<LeaveTypeDetailsDto>(LeaveTypeMockData.GetList()[0]);

            var handler = new GetLeaveTypeDetailsHandler(_mapper, _mockRepo.Object);

            var result = await handler.Handle(new GetLeaveTypeDetailsQuery(1), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<LeaveTypeDetailsDto>();
            result.ShouldBeEquivalentTo(expected);
        }
    }
}
