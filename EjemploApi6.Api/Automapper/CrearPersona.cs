using AutoMapper;
using EjemploApi6.Api.ViewModels.CrearPersona.Output;
using EjemploApi6.Api.ViewModels.GetDemo.Output;

namespace EjemploApi6.Api.Automapper;
/// <summary>
/// 
/// </summary>
public class CrearPersona : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public CrearPersona()
    {
        //Your maps
        CreateMap<Domain.Persona, CrearPersonaViewModelResponse>();
    }
}
