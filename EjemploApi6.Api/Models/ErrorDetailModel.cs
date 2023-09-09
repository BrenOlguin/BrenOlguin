using Newtonsoft.Json;

namespace EjemploApi6.Api.Models
{
    /// <summary>
    /// Detalle de error model
    /// </summary>
    [JsonObject(Title = "detalle_error_model")]
    public class ErrorDetailModel
    {
        /// <summary>
        /// ErrorDetailModel
        /// </summary>
        public ErrorDetailModel()
        {
            Errors = new List<Error>();
        }

        /// <summary>
        /// Descripción del código de error Http
        /// </summary>
        [JsonProperty(PropertyName = "eventId")]
        public string EventId { get; set; } = string.Empty;

        /// <summary>
        /// Detalle del error
        /// </summary>
        [JsonProperty(PropertyName = "detalle")]
        public string Detail { get; set; } = string.Empty;

        /// <summary>
        /// Correlation Id
        /// </summary>
        [JsonProperty(PropertyName = "correlationId")]
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Lista de errores
        /// </summary>
        [JsonProperty(PropertyName = "errores")]
        public List<Error> Errors { get; set; }
    }
}
