using EjemploApi6.DataAccess.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EjemploApi6.Application.Queries
{
    public class GetDemoQuery : IRequest<List<Domain.Demo>>
    {
    }

    public class GetDemoQueryHandler : IRequestHandler<GetDemoQuery, List<Domain.Demo>>
    {
        private readonly ILogger<GetDemoQueryHandler> _logger;
        private readonly IDemoRepository _demoRepository;

        public GetDemoQueryHandler(ILogger<GetDemoQueryHandler> logger, IDemoRepository demoRepository)
        {
            _logger = logger;
            _demoRepository = demoRepository;
        }
        public async Task<List<Domain.Demo>> Handle(GetDemoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace("entering to GetDemoQueryHandler");

                var result = await _demoRepository
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
}
