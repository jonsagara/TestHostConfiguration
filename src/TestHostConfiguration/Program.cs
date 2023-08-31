using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sagara.Core.Logging.Serilog;
using Serilog;
using TestHostConfiguration;

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
    hostAppBuilder.Logging.UseSerilog(configManager, ConfigureSerilog);

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

static void ConfigureSerilog(IConfiguration config, IServiceProvider services, LoggerConfiguration loggerConfig)
{
    // This is the .exe path in bin/{configuration}/{tfm}/
    var logDir = Directory.GetCurrentDirectory();

    // Log to the project directory.
    logDir = Path.Combine(logDir, @"..\..\..");
    logDir = Path.GetFullPath(logDir);
    Log.Logger.Information("Logging directory: {logDir}", logDir);

    // Serilog is our application logger. Default to Verbose. If we need to control this dynamically at some point
    //   in the future, we can: https://nblumhardt.com/2014/10/dynamically-changing-the-serilog-level/

    var logFilePathFormat = Path.Combine(logDir, "Logs", "log.txt");

    // Always write to a rolling file.
    loggerConfig
        .ReadFrom.Configuration(config)
        .ReadFrom.Services(services)
        .Enrich.With<UtcTimestampEnricher>()
        .WriteTo.Console()
        .WriteTo.File(logFilePathFormat, outputTemplate: "{UtcTimestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day);
}