using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LaFlorida.Data.Configuration;
using LaFlorida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LaFlorida.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<IdentityRole> ApplicationRoles { get; set; }
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Crop> Crops { get; set; }
        public virtual DbSet<Cycle> Cycles { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Withdraw> Withdraws { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            return await base.AddAsync(entity, cancellationToken);
        }

        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            return base.AddRangeAsync(entities, cancellationToken);
        }

        public override EntityEntry Attach(object entity)
        {
            return base.Attach(entity);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Load required data

            builder.ApplyConfiguration(new ApplicationUserConfig());
            builder.ApplyConfiguration(new ApplicationRoleConfig());
            builder.ApplyConfiguration(new ApplicationUserRoleConfig());
            builder.ApplyConfiguration(new JobConfig());

            // Identity customization to work with MySQL

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(85);

                entity.Property(e => e.UserName).HasMaxLength(128);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(128);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(128);
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(85);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.NormalizedName).HasMaxLength(128);
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(85);

                entity.Property(e => e.ProviderKey).HasMaxLength(85);
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(85);

                entity.Property(e => e.RoleId).HasMaxLength(85);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(85);

                entity.Property(e => e.LoginProvider).HasMaxLength(85);

                entity.Property(e => e.Name).HasMaxLength(85);
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(85);

                entity.Property(e => e.UserId).HasMaxLength(85);
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(85);

                entity.Property(e => e.RoleId).HasMaxLength(85);
            });

            // Application models

            builder.Entity<Cost>(entity =>
            {
                entity.HasKey(e => e.CostId)
                    .HasName("PK_Cost");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(e => e.ApplicationUser)
                    .WithMany(e => e.Costs)
                    .HasForeignKey(e => e.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cost_ApplicationUser");

                entity.HasOne(e => e.Cycle)
                    .WithMany(e => e.Costs)
                    .HasForeignKey(e => e.CycleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cost_Cycle");

                entity.HasOne(e => e.Job)
                    .WithMany(e => e.Costs)
                    .HasForeignKey(e => e.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cost_Job");
            });

            builder.Entity<Crop>(entity =>
            {
                entity.HasKey(e => e.CropId)
                    .HasName("PF_Crop");
            });

            builder.Entity<Cycle>(entity =>
            {
                entity.HasKey(e => e.CycleId)
                    .HasName("PK_Cycle");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IsComplete)
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsRent)
                    .HasDefaultValueSql("0");

                entity.HasOne(e => e.Crop)
                    .WithMany(e => e.Cycles)
                    .HasForeignKey(e => e.CropId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cycle_Crop");

                entity.HasOne(e => e.Lot)
                    .WithMany(e => e.Cycles)
                    .HasForeignKey(e => e.LotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cycle_Lot");
            });

            builder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK_Job");
            });

            builder.Entity<Lot>(entity =>
            {
                entity.HasKey(e => e.LotId)
                    .HasName("PK_Lot");
            });

            builder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId)
                    .HasName("PK_Sale");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Quintals)
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(e => e.Cycle)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.CycleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sale_Cycle");
            });

            builder.Entity<Withdraw>(entity =>
            {
                entity.HasKey(e => e.WithdrawId)
                    .HasName("PK_Withdraw");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("now()");

                entity.HasOne(e => e.ApplicationUser)
                    .WithMany(e => e.Withdraws)
                    .HasForeignKey(e => e.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Withdraw_ApplicationUser");

                entity.HasOne(e => e.Cycle)
                    .WithMany(e => e.Withdraws)
                    .HasForeignKey(e => e.CycleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Withdraw_Cycle");
            });
        }
    }
}
