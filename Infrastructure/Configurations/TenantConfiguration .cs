using Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.LogoUrl)
                   .HasMaxLength(500);
        }
    }
}
