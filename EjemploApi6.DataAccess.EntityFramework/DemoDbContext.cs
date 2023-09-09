using Microsoft.EntityFrameworkCore;

namespace EjemploApi6.DataAccess.EntityFramework
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Demo> Demos { get; set; }
        public DbSet<Domain.Persona> Personas { get; set; }
    }
}
