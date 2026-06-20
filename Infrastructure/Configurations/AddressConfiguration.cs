using Domain.Entities;
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
    public class AddressConfiguration
     : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Area).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Block).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Street).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Building).IsRequired().HasMaxLength(50);

            builder.HasOne<ApplicationUser>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId);
        }
    }
}
