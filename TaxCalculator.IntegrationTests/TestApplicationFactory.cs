using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace DataProcessing.WebAdmin.Api.IntegrationTests.TestTools;

public class TestApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        base.ConfigureWebHost(
            builder
                .ConfigureAppConfiguration(configurationBuilder =>
                    configurationBuilder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build()
                )
        );
}