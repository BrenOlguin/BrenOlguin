using Newtonsoft.Json;

namespace EjemploApi6.Api.ViewModels.CrearPersona.Output;
/// <summary>
/// 
/// </summary>
public class CrearPersonaViewModelResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }
}
