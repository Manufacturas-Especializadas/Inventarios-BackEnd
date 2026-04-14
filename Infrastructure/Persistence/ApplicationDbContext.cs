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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntryHeader>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.Details)
                      .WithOne(d => d.EntryHeader)
                      .HasForeignKey(d => d.EntryHeaderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EntryDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PartNumber).IsRequired().HasMaxLength(50);
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
        }
    }
}