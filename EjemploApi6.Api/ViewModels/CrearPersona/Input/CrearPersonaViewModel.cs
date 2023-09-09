using Microsoft.AspNetCore.Mvc;

namespace EjemploApi6.Api.ViewModels.CrearPersona.Input;
/// <summary>
/// 
/// </summary>
public class CrearPersonaViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [FromBody]
    public int Dni { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromBody]
    public string Nombre { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromBody]
    public string Apellido { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromBody]
    public int Edad { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [FromBody]
    public string Nacionalidad { get; set; }
}
