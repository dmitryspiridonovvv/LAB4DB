using FitnessCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Trainer> Trainers { get; set; } = default!;
        public DbSet<Zone> Zones { get; set; } = default!;
        public DbSet<MembershipPlan> MembershipPlans { get; set; } = default!;
        public DbSet<MembershipPlanZone> MembershipPlanZones { get; set; } = default!;
        public DbSet<MembershipSale> MembershipSales { get; set; } = default!;
        public DbSet<GroupClass> GroupClasses { get; set; } = default!;
        public DbSet<ClassSignup> ClassSignups { get; set; } = default!;
        public DbSet<Visit> Visits { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // связь M:N между планами и зонами
            modelBuilder.Entity<MembershipPlanZone>()
                .HasKey(pz => new { pz.MembershipPlanID, pz.ZoneID });

            modelBuilder.Entity<MembershipPlanZone>()
                .HasOne(pz => pz.MembershipPlan)
                .WithMany(p => p.PlanZones)
                .HasForeignKey(pz => pz.MembershipPlanID);

            modelBuilder.Entity<MembershipPlanZone>()
                .HasOne(pz => pz.Zone)
                .WithMany(z => z.PlanZones)
                .HasForeignKey(pz => pz.ZoneID);

            // Цена с типом decimal(10,2)
            modelBuilder.Entity<MembershipPlan>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");

            // Начальные данные
            modelBuilder.Entity<Zone>().HasData(
                new Zone { ZoneID = 1, Name = "Тренажёрный зал" },
                new Zone { ZoneID = 2, Name = "Бассейн" },
                new Zone { ZoneID = 3, Name = "Групповые занятия" }
            );
        }
    }
}
