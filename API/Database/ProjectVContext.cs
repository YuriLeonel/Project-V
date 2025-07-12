using API.Models;
using API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Database
{
    public class ProjectVContext : DbContext
    {
        public ProjectVContext(DbContextOptions<ProjectVContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyClients> CompanyClients { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ScheduleServices> ScheduleServices { get; set; }
        public DbSet<Reschedule> Reschedules { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.IdClient);

                entity.Property(c => c.ClientType).HasConversion(t => (int)t, t => (ClientTypeEnum)t);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.IdCompany);

                entity.HasOne(c=>c.Owner).WithOne(o=>o.OwnedCompany).HasForeignKey<Company>(c=>c.IdOwner).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CompanyClients>(entity =>
            {
                entity.HasKey(cc => cc.Id);

                entity.HasOne(cc => cc.Company).WithMany(c => c.CompanyClients).HasForeignKey(cc => cc.IdCompany).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cc => cc.Client).WithMany(c => c.CompanyClients).HasForeignKey(cc => cc.IdClient).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.IdSchedule);

                entity.Property(s => s.Staus).HasConversion(t => (int)t, t => (StatusScheduleEnum)t);

                entity.HasOne(s => s.Company).WithMany(c => c.Schedules).HasForeignKey(s => s.IdCompany).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Client).WithMany(c => c.Schedules).HasForeignKey(s => s.IdClient).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s=>s.IdService);

                entity.HasOne(s => s.Employee).WithMany(e => e.ServicesProvides).HasForeignKey(s => s.IdEmployee).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ScheduleServices>(entity =>
            {
                entity.HasKey(ss => ss.Id);

                entity.HasOne(ss => ss.Schedule).WithMany(s => s.ScheduleServices).HasForeignKey(ss => ss.IdSchedule).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ss => ss.Service).WithMany(s => s.ScheduleServices).HasForeignKey(ss => ss.IdService).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Reschedule>(entity =>
            {
                entity.HasKey(r => r.IdReschedule);

                entity.HasOne(r => r.Schedule).WithMany(s => s.Reschedules).HasForeignKey(r => r.IdSchedule).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(t => t.IdToken);

                entity.HasOne(t => t.Client).WithOne(c => c.Token).HasForeignKey<Token>(t => t.IdClient).OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
