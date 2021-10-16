using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class BenefitConfiguration : IEntityTypeConfiguration<Benefit>
    {

        public void Configure(EntityTypeBuilder<Benefit> builder)
        {
            // SQL config

            builder.ToTable("Benefit");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.IdPlan).IsRequired();

            builder.HasOne<Plan>(b => b.Plan)
            .WithMany(p => p.Benefits)
            .HasForeignKey(b => b.IdPlan);






        }
    }
}
