using Microsoft.Extensions.Hosting;
using Serilog;
using TestHostConfiguration.Hosting;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = MyHost.CreateApplicationBuilder(args);

    var host = builder.Build();
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
