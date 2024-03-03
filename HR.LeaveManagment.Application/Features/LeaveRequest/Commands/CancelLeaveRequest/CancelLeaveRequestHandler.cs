using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<CancelLeaveRequestHandler> _appLogger;

        public CancelLeaveRequestHandler(
            ILeaveRequestRepository leaveRequestRepository,
            IEmailSender emailSender,
            IAppLogger<CancelLeaveRequestHandler> appLogger)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _appLogger = appLogger ?? throw new ArgumentNullException(nameof(appLogger));
        }

        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest is null)
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            leaveRequest.Cancelled = true;

            // Re-evaluate the employee's allocations for the leave type

            SendEmail(leaveRequest);

            return Unit.Value;
        }

        private async void SendEmail(Domain.LeaveRequest request)
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

        private static EmailMessage BuildMail(Domain.LeaveRequest request)
        {

            EmailMessage email = new EmailMessage
            {
                To = [],
                Subject = "Leave Request Cancelled",
                Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                $"has been cancelled successfully."
            };

            return email;

        }
    }
}
