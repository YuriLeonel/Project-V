using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Database
{
    public class ProjectVContext : DbContext
    {
        public ProjectVContext(DbContextOptions<ProjectVContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
    }
}
