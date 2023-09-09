using EjemploApi6.DataAccess.Interface;
using EjemploApi6.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EjemploApi6.Application.Commands;

public class CrearPersonaCommand : IRequest<Persona>
{
    public int Dni { get; }
    public string Nombre { get;}
    public string Apellido { get; }
    public int Edad { get;}
    public string Nacionalidad { get; }


    public CrearPersonaCommand(int dni, string nombre, string apellido, int edad, string nacionalidad)
    {
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        Edad = edad;
        Nacionalidad = nacionalidad;
    }
}

public class CrearPersonaCommandHandler : IRequestHandler<CrearPersonaCommand, Persona>
{
    private readonly ILogger<CrearPersonaCommandHandler> _logger;
    private readonly IPersonaRepository _personaRepository;

    public CrearPersonaCommandHandler(ILogger<CrearPersonaCommandHandler> logger
        , IPersonaRepository personaRepository)
    {
        _logger = logger;
        _personaRepository = personaRepository;
    }

    public async Task<Persona> Handle(CrearPersonaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //_logger.LogTrace(ServiceEvents.EnteringToCrearPersonaCommandHandler, "Ejecutando Handle()");

            var persona = new Persona
            {
                Apellido = request.Apellido,
                Edad = request.Edad,
                Nacionalidad= request.Nacionalidad,
                Dni = request.Dni,
                Nombre = request.Nombre
            };

            var result = await _personaRepository.AddAsync(persona);
            return result;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ServiceEvents.ExceptionInCrearPersonaCommandHandler, ex,
            //    "Excepcion llamando a CreatePersonammandHandler()");
            throw;
        }
    }

}
