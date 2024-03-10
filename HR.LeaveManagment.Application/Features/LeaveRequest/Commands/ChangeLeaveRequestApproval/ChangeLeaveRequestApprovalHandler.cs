using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestApprovalHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
    {


        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<UpdateLeaveRequestHandler> _appLogger;

        public ChangeLeaveRequestApprovalHandler(
            ILeaveRequestRepository leaveRequestRepository,
            IEmailSender emailSender,
            IAppLogger<UpdateLeaveRequestHandler> appLogger
            )
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _appLogger = appLogger ?? throw new ArgumentNullException(nameof(appLogger));
        }

        public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

            var validator = new ChangeLeaveRequestApprovalValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                throw new BadRequestException("Invalid Change Approval Leave Request", validationResult);
            }


            leaveRequest.Approved = request.Approved;
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            // if the request is approved, gets and updates the employee's allocations

            await SendEmail(leaveRequest);

            return Unit.Value;
        }

        private async Task SendEmail(Domain.LeaveRequest request)
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
                Subject = "Leave Request Approval status updated",
                Body = $"Your approval status for your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                $"has been updated."
            };

            return email;

        }
    }
}
