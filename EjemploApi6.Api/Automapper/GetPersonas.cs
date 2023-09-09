using AutoMapper;
using EjemploApi6.Api.ViewModels.CrearPersona.Output;
using EjemploApi6.Api.ViewModels.GetPersonas.Output;

namespace EjemploApi6.Api.Automapper
{
    public class GetPersonas : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public GetPersonas()
        {
            //Your maps
            CreateMap<Domain.Persona, GetPersonasViewModelResponse>();
        }
    }
}
