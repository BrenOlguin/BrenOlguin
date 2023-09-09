using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using EjemploApi6.Api.Clients;
using EjemploApi6.Api.Filters;
using EjemploApi6.Api.Middleware;
using EjemploApi6.Api.Models;
using EjemploApi6.Api.Swagger;
using EjemploApi6.Application;
using EjemploApi6.Common;
using EjemploApi6.Common.Configurations;
using EjemploApi6.DataAccess.EntityFramework;
using EjemploApi6.DataAccess.Interface;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ModelStateValidateAttribute));
        options.Filters.Add(typeof(ExceptionsAttribute));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetailModel), StatusCodes.Status422UnprocessableEntity));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetailModel), StatusCodes.Status500InternalServerError));
    })
    .AddNewtonsoftJson();


#region Serilog

builder.Host.UseSerilog((_, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithCorrelationIdHeader(AppConstants.XCorrelationIdName)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
        .WithDefaultDestructurers()
        .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }))
    .WriteTo.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

#endregion

#region Services for entity framework

builder.Services.AddDbContext<DemoDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

#endregion

#region Correlation Ids

builder.Services.AddCorrelate(options => options.RequestHeaders = new[] { AppConstants.XCorrelationIdName });

#endregion Correlation Ids

#region Servicio de Logs Request/Response

builder.Services.AddTransient<RequestResponseLoggingMiddleware>();

#endregion Servicio de Logs Request/Response

#region HttpClient Configurations

builder.Services.AddHttpClientConfiguration(builder.Configuration);

#endregion

#region Propagacion de Headers

builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add(AppConstants.UserHeaderName);
    options.Headers.Add(AppConstants.ChannelHeaderName);
    options.Headers.Add(AppConstants.ApplicationHeaderName);
});

#endregion Propagacion de Headers

#region Versionado de la Api

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddApiVersioning(
    options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    });

#endregion Versionado de la Api

#region Configuracion ApiBehaviorOptions

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressConsumesConstraintForFormFileParameters = true;
});

#endregion Configuracion ApiBehaviorOptions

#region Autommaper

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));

#endregion

#region Open Api (swagger)

builder.Services.AddSwaggerGenForService();

builder.Services.AddSwaggerGenNewtonsoftSupport();

#endregion Open Api (swagger)

#region HealthChecks

builder.Services.AddHealthChecks()
    .AddSqlServer(
    connectionString: builder.Configuration["ConnectionStrings:DefaultConnection"],
    healthQuery: "SELECT 1;",
    name: "Sql Server",
    failureStatus: HealthStatus.Unhealthy,
    tags: new[] { "db", "sql", "sqlserver" });

builder.Services.AddHealthChecksUI().AddInMemoryStorage();



#endregion

#region IOption

builder.Services.Configure<OpenApiInfoConfigurationOptions>(builder.Configuration.GetSection("OpenApiInfo"));

#endregion IOption

#region Add MediatR

builder.Services.AddMediatR(typeof(MediatorUpInjection).Assembly);

#endregion

#region Configuration Injection Dependency

builder.Services.AddTransient<IDemoRepository, DemoRepository>();
builder.Services.AddTransient<IPersonaRepository, PersonaRepository>();

#endregion

var app = builder.Build();

app.UseCorrelate();
app.UseHeaderPropagation();

app.UseRequestResponseLogging();

app.UseSwagger(c => { c.SerializeAsV2 = true; });
app.UseSwaggerUI(app.Services.GetRequiredService<IApiVersionDescriptionProvider>().SwaggerOptionUi);

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(config =>
{
    config.MapHealthChecksUI(options =>
    {
        options.UIPath = "/health-ui";
    });
    config.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});


app.UseHealthChecks("/health");

#region Remover solo para TEST
using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
var context = serviceScope?.ServiceProvider.GetRequiredService<DemoDbContext>();
context?.Database.EnsureCreated();
#endregion

app.Run();
