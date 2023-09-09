using EjemploApi6.DataAccess.EntityFramework;

namespace EjemploApi6.Test.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(DemoDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Mock de tablas aqui

            context.SaveChanges();
        }
    }
}