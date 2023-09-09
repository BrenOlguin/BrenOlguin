using EjemploApi6.Test.Infrastructure;
using Xunit;

namespace EjemploApi6.Test.Integration
{
    [Collection(ServerFixtureCollection.Name)]
    public class UnitTest1
    {
        private readonly ServerFixture _server;

        public UnitTest1(ServerFixture server)
        {
            _server = server;
        }

        [Fact]
        public void Test1()
        {
            var server = _server.HttpServer.TestServer;
            var client = server?.CreateClient();
        }
    }
}