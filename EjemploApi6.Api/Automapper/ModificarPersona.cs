using AutoMapper;
using EjemploApi6.Api.ViewModels.ModificarPersona.Output;

namespace EjemploApi6.Api.Automapper;
/// <summary>
/// 
/// </summary>
public class ModificarPersona : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ModificarPersona()
    {
        //Your maps
        CreateMap<Domain.Persona, ModificarPersonaViewModelResponse>();
    }
}
