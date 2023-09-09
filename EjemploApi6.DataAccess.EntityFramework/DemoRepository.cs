using EjemploApi6.DataAccess.Interface;

namespace EjemploApi6.DataAccess.EntityFramework
{
    public class DemoRepository : GenericRepository<Domain.Demo>, IDemoRepository
    {
        public DemoRepository(DemoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
