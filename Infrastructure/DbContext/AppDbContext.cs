using Application.Interfaces.Common;
using Domain.Entities;
using Domain.Entities.Common;
using Domain.Entities.Tenants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbContext
{

    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        private readonly ICurrentUserService _currentUser;
        private readonly ICurrentTenantService _currentTenant;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser,
            ICurrentTenantService currentTenant) : base(options)
        {
            _currentUser = currentUser;
            _currentTenant = currentTenant;
        }
        #region override methods
        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyGlobalFilters(builder);
        }
        #endregion

        #region DbSets
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        #endregion

        #region private methods
        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker
                .Entries<AuditableEntity>()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                var userId = _currentUser.UserId;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId ?? new Guid();
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                }

                // ✅ SOFT DELETE
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.SoftDelete();
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;

                   // entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;
                   //entry.Property(nameof(AuditableEntity.CreatedBy)).IsModified = false;

                }
            }
        }



        private void ApplyGlobalFilters(ModelBuilder builder)
        {
            // ✅ Tenant Filter
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseTenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetTenantFilter),
                           BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(this, new object[] { builder });
                }
            }

            // ✅ Soft Delete Filter
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter),
                           BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(this, new object[] { builder });
                }
            }
        }
        private void SetSoftDeleteFilter<TEntity>(ModelBuilder builder)
    where TEntity : AuditableEntity
        {
            builder.Entity<TEntity>()
                .HasQueryFilter(e => !e.IsDeleted);
        }
        private void SetTenantFilter<TEntity>(ModelBuilder builder)
    where TEntity : BaseTenantEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e =>
                !_currentTenant.TenantId.HasValue ||
                _currentTenant.IsSuperAdmin ||
                e.TenantId == _currentTenant.TenantId);
        }

        #endregion
    }
    }
