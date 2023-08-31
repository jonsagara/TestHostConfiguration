using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Hello, world.");

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
