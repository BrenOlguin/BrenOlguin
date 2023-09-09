using Polly;
using Polly.Extensions.Http;
using Serilog;
using System.Net;

namespace EjemploApi6.Api.Clients
{
    /// <summary>
    /// Main Polly Policy Builder
    /// </summary>
    public class PollyPolicyBuilder
    {
        /// <summary>
        /// Retry Policy for general HttpClient usage
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, context) =>
                {
                    Log.Logger.Error("Retry StatusCode:{StatusCode}, Url: {RequestUri}", exception?.Result?.StatusCode, exception?.Result?.RequestMessage?.RequestUri);
                });
        }
    }
}
