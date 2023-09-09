using System.Diagnostics;

namespace EjemploApi6.Test.Infrastructure
{
    public class ServerFixture : IDisposable
    {
        public ServerFixture()
        {
            Debug.Write("ServerFixture Constructor - Se ejecuta una sola vez antes de la ejecución de los test.");

            HttpServer = new ServerMock();
            WireMock = new WireMockHelper(HttpServer.UrlHostMock, HttpServer.PortMock);
        }

        public ServerMock HttpServer { get; }

        public WireMockHelper WireMock { get; set; }

        public void Dispose()
        {
            Debug.Write("Disposes only once per test.");
            WireMock.Stop();
            WireMock.Dispose();
            HttpServer.Dispose();
        }
    }
}