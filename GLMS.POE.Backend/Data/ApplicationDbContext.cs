using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GLMS.POE.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        //public DbSet<Client> Clients => Set<Client>();
        //public DbSet<Contract> Contracts => Set<Contract>();
        //public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ContactDetails).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Region).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServiceLevel).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SignedAgreementPath).HasMaxLength(500);
                entity.Property(e => e.SignedAgreementFileName).HasMaxLength(255);
                entity.Property(e => e.Status)
                    .HasConversion<string>();

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Contracts)
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CostUsd).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CostZar).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ExchangeRateUsed).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Status)
                    .HasConversion<string>();

                entity.HasOne(e => e.Contract)
                    .WithMany(c => c.ServiceRequests)
                    .HasForeignKey(e => e.ContractId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Oceanic Freight Ltd", ContactDetails = "info@oceanicfreight.com | +27 21 555 0100", Region = "Africa", CreatedAt = new DateTime(2025, 1, 1) },
                new Client { Id = 2, Name = "EuroCargo GmbH", ContactDetails = "ops@eurocargo.de | +49 30 555 0200", Region = "Europe", CreatedAt = new DateTime(2025, 1, 1) },
                new Client { Id = 3, Name = "Pacific Rim Shipping", ContactDetails = "contact@pacificrim.com | +65 6555 0300", Region = "Asia-Pacific", CreatedAt = new DateTime(2025, 1, 1) }
            );

            modelBuilder.Entity<Contract>().HasData(
                new Contract
                {
                    Id = 1,
                    ClientId = 1,
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2026, 1, 1),
                    Status = ContractStatus.Active,
                    ServiceLevel = "Premium",
                    CreatedAt = new DateTime(2025, 1, 1)
                },
                new Contract
                {
                    Id = 2,
                    ClientId = 2,
                    StartDate = new DateTime(2024, 6, 1),
                    EndDate = new DateTime(2025, 6, 1),
                    Status = ContractStatus.Expired,
                    ServiceLevel = "Standard",
                    CreatedAt = new DateTime(2024, 6, 1)
                },
                new Contract
                {
                    Id = 3,
                    ClientId = 3,
                    StartDate = new DateTime(2025, 3, 1),
                    EndDate = new DateTime(2027, 3, 1),
                    Status = ContractStatus.Draft,
                    ServiceLevel = "Enterprise",
                    CreatedAt = new DateTime(2025, 3, 1)
                }
            );
        }
    }
}
