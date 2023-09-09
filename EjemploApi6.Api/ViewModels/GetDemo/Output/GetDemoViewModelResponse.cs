using Newtonsoft.Json;

namespace EjemploApi6.Api.ViewModels.GetDemo.Output
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDemoViewModelResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
