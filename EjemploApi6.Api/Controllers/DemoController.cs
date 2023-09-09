using AutoMapper;
using EjemploApi6.Api.ViewModels.GetDemo.Output;
using EjemploApi6.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace EjemploApi6.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route(RouteRoot)]
    public class DemoController : ControllerBase
    {
        private const string RouteRoot = "api/v{version:apiVersion}";

        private readonly ILogger<DemoController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public DemoController(ILogger<DemoController> logger
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
        [HttpGet("demo")]
        [SwaggerOperation(Summary = "Obtener una demo.", Tags = new[] { "demo" })]
        [ProducesResponseType(typeof(List<GetDemoViewModelResponse>), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        public async Task<IActionResult> GetDemo()
        {
            _logger.LogDebug("entering to demo controller");

            var domainDemo = await _mediator.Send(new GetDemoQuery());

            var viewModelResponse = _mapper.Map<List<GetDemoViewModelResponse>>(domainDemo);

            return Ok(viewModelResponse);
        }
    }
}