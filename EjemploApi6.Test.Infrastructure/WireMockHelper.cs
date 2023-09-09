using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;

namespace EjemploApi6.Test.Infrastructure
{
    public class WireMockHelper : IDisposable
    {
        private readonly IWireMockLogger _logger = new WireMockConsoleLogger();

        public WireMockHelper(string urlHost, string port)
        {
            ServiceMock = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] { $"{urlHost}:{port}" },
                StartAdminInterface = true,
                Logger = _logger
            });
        }

        public WireMockServer ServiceMock { get; set; }

        public void Dispose()
        {
            ServiceMock.Stop();
            ServiceMock.Dispose();
        }

        public void ResetMapping()
        {
            ServiceMock.ResetMappings();
        }

        public void Stop()
        {
            ServiceMock.Stop();
        }
    }
}
