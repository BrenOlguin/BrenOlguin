using AutoMapper;
using EjemploApi6.Api.ViewModels.GetDemo.Output;

namespace EjemploApi6.Api.Automapper
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDemoProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public GetDemoProfile()
        {
            //Your maps
            CreateMap<Domain.Demo, GetDemoViewModelResponse>();
        }
    }
}
