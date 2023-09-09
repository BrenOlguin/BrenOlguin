using EjemploApi6.Common.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace EjemploApi6.Api.Swagger
{
    /// <summary>
    /// SwaggerDefaultValues
    /// </summary>
    public static class SwaggerGenSettings
    {
        /// <summary>
        /// AddSwaggerGen
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerGenForService(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    // resuelve el servicio IApiVersionDescriptionProvider
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    //Agrega un documento swagger para cada versión de API descubierta
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, services));
                    }

                    //integramos xml comments
                    if (!string.IsNullOrEmpty(XmlCommentsFilePath()))
                    {
                        options.IncludeXmlComments(XmlCommentsFilePath());
                    }

                    options.OperationFilter<SwaggerOperationFilter>();
                    options.EnableAnnotations();
                    options.CustomSchemaIds(type => type.FullName);
                });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IServiceCollection services)
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;

                var openApiInfoConfiguration = services.BuildServiceProvider()
                         .GetRequiredService<IOptions<OpenApiInfoConfigurationOptions>>();

                var info = new OpenApiInfo
                {
                    Title = openApiInfoConfiguration.Value.Title,
                    Version = "1.0.0",
                    Description = $"Release: {version}",
                    Contact = new OpenApiContact { Email = openApiInfoConfiguration.Value.Contact },
                    TermsOfService = new Uri(openApiInfoConfiguration.Value.TermsOfService)
                };

                if (description.IsDeprecated) info.Description += " Esta versión de API esta deprecated";

                return info;
            }
            catch (Exception ex)
            {
                Log.Error("Error creando configuracion para OpenApi" + ex.Message);
                throw new Exception("Error creando configuracion para OpenApi: " + ex.Message);
            }
        }

        private static string XmlCommentsFilePath()
        {
            string path = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            return path;
        }
    }
}
