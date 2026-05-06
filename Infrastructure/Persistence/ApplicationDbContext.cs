using Application.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ProductionLine> ProductionLines => Set<ProductionLine>();

        public DbSet<EntryHeader> EntryHeaders => Set<EntryHeader>();

        public DbSet<EntryDetail> EntryDetails => Set<EntryDetail>();

        public DbSet<ExitHeader> ExitHeaders => Set<ExitHeader>();

        public DbSet<ExitDetail> ExitDetails => Set<ExitDetail>();

        public DbSet<ShippingRelease> ShippingReleases => Set<ShippingRelease>();

        public DbSet<ShippingScan> ShippingScans => Set<ShippingScan>();

        public DbSet<ExitReportLog> ExitReportLogs => Set<ExitReportLog>();

        public DbSet<ExitReportLogDetail> ExitReportLogDetails => Set<ExitReportLogDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntryHeader>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ShopOrder)
                        .HasMaxLength(50)
                        .IsRequired(false);

                entity.HasMany(e => e.Details)
                      .WithOne(d => d.EntryHeader)
                      .HasForeignKey(d => d.EntryHeaderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EntryDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PartNumber).IsRequired().HasMaxLength(50);

                entity.Property(e => e.BoxesQuantity)
                       .IsRequired(false);
            });

            modelBuilder.Entity<ExitHeader>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ShopOrder1).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ShopOrder2).HasMaxLength(50);
                entity.Property(e => e.ShopOrder3).HasMaxLength(50);
                entity.Property(e => e.ShopOrder4).HasMaxLength(50);
                entity.Property(e => e.ShopOrder5).HasMaxLength(50);
                entity.Property(e => e.ShopOrder6).HasMaxLength(50);

                entity.HasMany(e => e.Details)
                      .WithOne(d => d.ExitHeader)
                      .HasForeignKey(d => d.ExitHeaderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExitDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PartNumber).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<ProductionLine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<ShippingRelease>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShopOrder).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PartNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PackerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasMany(e => e.Scans)
                        .WithOne(s => s.ShippingRelease)
                        .HasForeignKey(s => s.ShippingReleaseId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShippingScan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ScannedLabelId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ScannedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}