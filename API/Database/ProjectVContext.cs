using API.Models;
using API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Database
{
    public class ProjectVContext : DbContext
    {
        public ProjectVContext(DbContextOptions<ProjectVContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.IdClient);

                entity.Property(c => c.ClientType)
                .HasConversion(t => (int)t,
                t => (ClientType)t);
            });
        }
    }
}
