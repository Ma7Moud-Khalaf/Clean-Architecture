using Domain.Entities.Tenants;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicationUserConfiguration
    : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.IsGuest)
               .HasDefaultValue(false);

        builder.HasIndex(x => x.PhoneNumber)
               .IsUnique(false);

        builder.HasOne<Tenant>()
               .WithMany()
               .HasForeignKey(x => x.TenantId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}