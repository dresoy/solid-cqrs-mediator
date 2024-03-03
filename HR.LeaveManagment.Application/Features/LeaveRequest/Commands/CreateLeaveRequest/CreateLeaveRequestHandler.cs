using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<UpdateLeaveRequestHandler> _appLogger;

        public CreateLeaveRequestHandler(
            IMapper mapper,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IEmailSender emailSender,
            IAppLogger<UpdateLeaveRequestHandler> appLogger
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _appLogger = appLogger ?? throw new ArgumentNullException(nameof(appLogger));
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestValidator(_leaveTypeRepository);
            var validationResults = await validator.ValidateAsync(request);

            if (validationResults.Errors.Count > 0)
            {
                throw new BadRequestException("Invalid Leave Request", validationResults);
            }

            // Get requesting employee's id

            // Check on employee's allocation

            // if allocations aren't enough, return validation error with message

            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            SendEmail(request);

            return Unit.Value;
        }

        private async void SendEmail(CreateLeaveRequestCommand request)
        {
            try
            {
                await _emailSender.SendEmail(BuildMail(request));
            }
            catch (Exception ex)
            {
                _appLogger.LogWarning(ex.Message);
            }


        }

        private static EmailMessage BuildMail(CreateLeaveRequestCommand request)
        {

            EmailMessage email = new EmailMessage
            {
                To = [],
                Subject = "Leave Request Submitted",
                Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                $"has been updated successfully."
            };

            return email;

        }
    }
}
