using EjemploApi6.Test.Infrastructure;
using Xunit;

namespace EjemploApi6.Test.Integration
{
    [CollectionDefinition(Name)]
    public class ServerFixtureCollection : ICollectionFixture<ServerFixture>
    {
        public const string Name = "ServerFixture collection";
    }
}
