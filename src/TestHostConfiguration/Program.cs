using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    //var hostAppBuilder = Host.CreateApplicationBuilder(args);
    var configManager = new ConfigurationManager();
    configManager.AddJsonFile("hostsettings.json", optional: true);
    var hostAppBuilderSettings = new HostApplicationBuilderSettings
    {
        Args = args,
        Configuration = configManager,
    };
    var hostAppBuilder = new HostApplicationBuilder(hostAppBuilderSettings);

    var host = hostAppBuilder.Build();
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Error(ex, "Unhandled exception in Program.Main.");

    return -1;
}
finally
{
    Log.CloseAndFlush();
}
