using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<UpdateLeaveRequestHandler> _appLogger;

        public UpdateLeaveRequestHandler(
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

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

            var validator = new UpdateLeaveRequestValidator(_leaveTypeRepository, _leaveRequestRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            _mapper.Map(request, leaveRequest);

            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            SendEmail(request);

            return Unit.Value;
        }

        private async void SendEmail(UpdateLeaveRequestCommand request)
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

        private static EmailMessage BuildMail(UpdateLeaveRequestCommand request)
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
