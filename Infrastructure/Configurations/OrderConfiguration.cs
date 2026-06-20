using Domain.Entities;
using Domain.Entities.Tenants;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class OrderConfiguration
    : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.HasOne<ApplicationUser>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Address>()
                   .WithMany()
                   .HasForeignKey(x => x.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Tenant>()
                   .WithMany()
                   .HasForeignKey(x => x.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TenantId);
        }
    }
}
