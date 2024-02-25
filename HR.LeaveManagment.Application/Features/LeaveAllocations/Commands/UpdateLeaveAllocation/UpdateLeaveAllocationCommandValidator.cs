﻿using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public UpdateLeaveAllocationCommandValidator(
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveAllocationRepository leaveAllocationRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;

            // TODO: Add logic to validate the the max number of days should not be greater than
            // the number set by the LeaveType

            RuleFor(p => p.NumberOfDays)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}");

            RuleFor(p => p.Period)
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(LeaveTypeMustExists)
                .WithMessage("{PropertyName} does not exists");

            RuleFor(p => p.Id)
                .NotNull()
                .MustAsync(LeaveAllocationMustExists)
                .WithMessage("{PropertyName} must be present");
        }

        private async Task<bool> LeaveAllocationMustExists(int id, CancellationToken token)
        {
            var exists = await _leaveAllocationRepository.GetByIdAsync(id);
            return exists != null;
        }

        private async Task<bool> LeaveTypeMustExists(int leaveTypeId, CancellationToken token)
        {
            var exists = await _leaveTypeRepository.GetByIdAsync(leaveTypeId);
            return exists != null;
        }
    }
}
