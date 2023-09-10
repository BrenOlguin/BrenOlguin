using EjemploApi6.DataAccess.Interface;
using EjemploApi6.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EjemploApi6.Application.Commands;

public class ModificarPersonaCommand : IRequest<Persona>
{
    public int Id { get; }
    public int Dni { get; }
    public string Nombre { get; }
    public string Apellido { get; }
    public int Edad { get; }
    public string Nacionalidad { get; }


    public ModificarPersonaCommand(int id, int dni, string nombre, string apellido, int edad, string nacionalidad)
    {
        Id = id;
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        Edad = edad;
        Nacionalidad = nacionalidad;
    }
}

public class ModificarPersonaCommandHandler : IRequestHandler<ModificarPersonaCommand, Persona>
{
    private readonly ILogger<ModificarPersonaCommandHandler> _logger;
    private readonly IPersonaRepository _personaRepository;

    public ModificarPersonaCommandHandler(ILogger<ModificarPersonaCommandHandler> logger
        , IPersonaRepository personaRepository)
    {
        _logger = logger;
        _personaRepository = personaRepository;
    }

    public async Task<Persona> Handle(ModificarPersonaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //_logger.LogTrace(ServiceEvents.EnteringToModificarPersonaCommandHandler, "Ejecutando Handle()");

            var persona = await _personaRepository.All().FirstOrDefaultAsync(persona => persona.Id == request.Id);

            persona.Dni = request.Dni;
            persona.Apellido = request.Apellido;
            persona.Edad = request.Edad;
            persona.Nacionalidad =  request.Nacionalidad;
            persona.Nombre = request.Nombre;

            await _personaRepository.UpdateAsync(persona);

            return persona;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ServiceEvents.ExceptionInModificarPersonaCommandHandler, ex,
            //    "Excepcion llamando a CreatePersonammandHandler()");
            throw;
        }
    }

}
