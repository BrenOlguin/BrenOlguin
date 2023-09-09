using EjemploApi6.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EjemploApi6.Test.Infrastructure
{
    public class ServerMock : IDisposable
    {
        public DemoDbContext? DemoCtx { get; }
        public IConfigurationRoot? Configuration { get; private set; }
        public string PortMock { get; set; } = string.Empty;
        public string UrlHostMock { get; set; } = string.Empty;
        public TestServer? TestServer { get; }


        public ServerMock()
        {
            try
            {
                var webApplicationFactory = CreateWebApplicationFactory();
                var scope = webApplicationFactory.Server.Services.CreateScope();
                var services = scope.ServiceProvider;

                TestServer = webApplicationFactory.Server;

                DemoCtx = services.GetRequiredService<DemoDbContext>();

                DbInitializer.Initialize(DemoCtx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Dispose()
        {
        }

        private WebApplicationFactory<Program> CreateWebApplicationFactory()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Staging.json", false, true)
                .Build();

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, conf) =>
                    {
                        conf.AddConfiguration(Configuration);
                    });
                    builder.ConfigureTestServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d =>
                            d.ServiceType == typeof(DbContextOptions<DemoDbContext>));

                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<DemoDbContext>(options =>
                            options.UseInMemoryDatabase(databaseName: "SampleDB"));
                    });
                });

            FillConfiguration();

            return application;
        }

        private void FillConfiguration()
        {
            PortMock = Configuration.GetValue<string>("PortMock");
            UrlHostMock = Configuration.GetValue<string>("UrlHostMock");
        }
    }
}
