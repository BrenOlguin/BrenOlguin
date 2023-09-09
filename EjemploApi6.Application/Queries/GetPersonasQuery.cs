using EjemploApi6.DataAccess.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EjemploApi6.Application.Queries;

public class GetPersonasQuery : IRequest<List<Domain.Persona>>
{
}

public class GetPersonasQueryHandler : IRequestHandler<GetPersonasQuery, List<Domain.Persona>>
{
    private readonly ILogger<GetPersonasQueryHandler> _logger;
    private readonly IPersonaRepository _personaRepository;

    public GetPersonasQueryHandler(ILogger<GetPersonasQueryHandler> logger, IPersonaRepository personaRepository)
    {
        _logger = logger;
        _personaRepository = personaRepository;
    }
    public async Task<List<Domain.Persona>> Handle(GetPersonasQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace("entering to GetPersonasQueryHandler");

            var result = await _personaRepository
                .All()
                .ToListAsync(cancellationToken);

            return result;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
