using AutoMapper;
using EjemploApi6.Api.Automapper;
using EjemploApi6.Api.ViewModels.CrearPersona.Input;
using EjemploApi6.Api.ViewModels.CrearPersona.Output;
using EjemploApi6.Api.ViewModels.GetDemo.Output;
using EjemploApi6.Api.ViewModels.GetPersonas.Output;
using EjemploApi6.Api.ViewModels.ModificarPersona.Input;
using EjemploApi6.Api.ViewModels.ModificarPersona.Output;
using EjemploApi6.Application.Commands;
using EjemploApi6.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using YamlDotNet.Core.Tokens;

namespace EjemploApi6.Api.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route(RouteRoot)]
public class PersonaController : ControllerBase
{
    private const string RouteRoot = "api/v{version:apiVersion}";

    private readonly ILogger<PersonaController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    public PersonaController(ILogger<PersonaController> logger
    , IMapper mapper
        , IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost("persona")]
    [SwaggerOperation(Summary = "Crear una persona.", Tags = new[] { "persona" })]
    [ProducesResponseType(typeof(CrearPersonaViewModelResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
    public async Task<IActionResult> CrearPersona(CrearPersonaViewModel datosDeEntrada)
    {
        _logger.LogDebug("entering to persona controller");

        var response = await _mediator.Send(new CrearPersonaCommand(datosDeEntrada.Dni, datosDeEntrada.Nombre, datosDeEntrada.Apellido, datosDeEntrada.Edad, datosDeEntrada.Nacionalidad));

        var viewModelResponse = _mapper.Map<CrearPersonaViewModelResponse>(response);

        return Ok(viewModelResponse);
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("personas")]
    [SwaggerOperation(Summary = "Muestra listado de personas.", Tags = new[] { "persona" })]
    [ProducesResponseType(typeof(GetPersonasViewModelResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
    public async Task<IActionResult> GetPersonas()
    {
        _logger.LogDebug("entering to get personas controller");

        var response = await _mediator.Send(new GetPersonasQuery());

        var viewModelResponse = _mapper.Map<List<GetPersonasViewModelResponse>>(response);

        return Ok(viewModelResponse);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut("persona")]
    [SwaggerOperation(Summary = "Modificar datos de una persona.", Tags = new[] { "persona" })]
    [ProducesResponseType(typeof(ModificarPersonaViewModelResponse), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
    public async Task<IActionResult> ModificarPersona(ModificarPersonaViewModel model)
    {
        _logger.LogDebug("entering to modificar personas controller");

        var response = await _mediator.Send(new ModificarPersonaCommand(model.Id,model.Dni,model.Nombre,model.Apellido,model.Edad,model.Nacionalidad));

        var viewModelResponse = _mapper.Map<ModificarPersonaViewModelResponse>(response);

        return Ok(viewModelResponse);
    }


}
