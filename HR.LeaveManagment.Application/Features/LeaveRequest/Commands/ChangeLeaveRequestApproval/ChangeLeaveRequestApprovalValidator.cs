using FluentValidation;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval
{
    public class ChangeLeaveRequestApprovalValidator : AbstractValidator<ChangeLeaveRequestApprovalCommand>
    {

        public ChangeLeaveRequestApprovalValidator()
        {
            RuleFor(p => p.Approved)
                    .NotNull()
                    .WithMessage("Approval status cannot be null");
        }
    }
}
