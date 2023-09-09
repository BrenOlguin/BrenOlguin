using EjemploApi6.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploApi6.DataAccess.EntityFramework;

public class PersonaRepository : GenericRepository<Domain.Persona>, IPersonaRepository
{
    public PersonaRepository(DemoDbContext dbContext) : base(dbContext)
    {
    }
}
