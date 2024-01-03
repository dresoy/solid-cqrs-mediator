using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {

            builder.Property(q => q.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            builder.HasData(
                new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 18, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}
