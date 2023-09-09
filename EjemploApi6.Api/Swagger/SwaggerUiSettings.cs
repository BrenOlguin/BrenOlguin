using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EjemploApi6.Api.Swagger
{
    /// <summary>
    /// SwaggerUiSettings Class
    /// </summary>
    public static class SwaggerUiSettings
    {
        /// <summary>
        /// Configuracion de la UI de swagger
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        public static void SwaggerOptionUi(this IApiVersionDescriptionProvider provider, SwaggerUIOptions options)
        {
            //Crea un endpoint swagger (definicion) para cada versión de API descubierta
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }

            //Colapsa la seccion Models cuando se renderiza la UI
            options.DefaultModelsExpandDepth(0);

            //Establece el path de inicio de la UI de swagger
            options.RoutePrefix = "swagger";
        }
    }
}
