using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TestHostConfiguration;

public static class SerilogExtensions
{
    /// <summary>
    /// Sets Serilog as the logging provider.
    /// </summary>
    /// <remarks>
    /// <para>This is (hopefully) a temporary replacement for <c>Serilog.Extensions.Hosting.UseSerilog</c>. It's necessary
    /// to support working with the new <c>Host.CreateApplicationBuilder()</c> pattern being rolled out in .NET 7, which 
    /// does not currently support the <c>IHostBuilder</c> interface.</para>
    /// <para>See: https://github.com/dotnet/runtime/discussions/81090#discussioncomment-4784551</para>
    /// </remarks>
    /// <param name="builder">The logging builder to configure.</param>
    /// <param name="config">The IConfiguration from which Serilog will attempt to read its configuration.</param>
    /// <param name="configureLogger">The delegate for configuring the Serilog.LoggerConfiguration that will be used 
    /// to construct a Serilog.Core.Logger.</param>
    /// <param name="preserveStaticLogger">Indicates whether to preserve the value of Serilog.Log.Logger.</param>
    /// <param name="writeToProviders">By default, Serilog does not write events to Microsoft.Extensions.Logging.ILoggerProviders 
    /// registered through the Microsoft.Extensions.Logging API. Normally, equivalent Serilog sinks are used in place of providers. 
    /// Specify true to write events to all providers.</param>
    /// <returns>The logging builder.</returns>
    public static ILoggingBuilder UseSerilog(this ILoggingBuilder builder, IConfiguration config, Action<IConfiguration, IServiceProvider, LoggerConfiguration> configureLogger, bool preserveStaticLogger = false, bool writeToProviders = false)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureLogger);

        IConfiguration config2 = config;

        builder.Services.AddSerilog((serviceProvider, loggerConfiguration) =>
            {
                configureLogger(config2, serviceProvider, loggerConfiguration);
            },
            preserveStaticLogger: preserveStaticLogger,
            writeToProviders: writeToProviders
            );

        return builder;
    }

    /// <summary>
    /// Sets Serilog as the logging provider.
    /// </summary>
    /// <remarks>
    /// <para>This is (hopefully) a temporary replacement for <c>Serilog.Extensions.Hosting.UseSerilog</c>. It's necessary
    /// to support working with the new <c>Host.CreateApplicationBuilder()</c> pattern being rolled out in .NET 7, which 
    /// does not currently support the <c>IHostBuilder</c> interface.</para>
    /// <para>See: https://github.com/dotnet/runtime/discussions/81090#discussioncomment-4784551</para>
    /// </remarks>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> to configure.</param>
    /// <param name="configureLogger">The delegate for configuring the Serilog.LoggerConfiguration that will be used 
    /// to construct a Serilog.Core.Logger.</param>
    /// <param name="preserveStaticLogger">Indicates whether to preserve the value of Serilog.Log.Logger.</param>
    /// <param name="writeToProviders">By default, Serilog does not write events to Microsoft.Extensions.Logging.ILoggerProviders 
    /// registered through the Microsoft.Extensions.Logging API. Normally, equivalent Serilog sinks are used in place of providers. 
    /// Specify true to write events to all providers.</param>
    /// <returns>The host application builder.</returns>
    public static HostApplicationBuilder UseSerilog(this HostApplicationBuilder builder, Action<IConfiguration, IServiceProvider, LoggerConfiguration> configureLogger, bool preserveStaticLogger = false, bool writeToProviders = false)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureLogger);

        IConfiguration config2 = builder.Configuration;

        builder.Services.AddSerilog(
            (serviceProvider, loggerConfiguration) => configureLogger(config2, serviceProvider, loggerConfiguration),
            preserveStaticLogger: preserveStaticLogger,
            writeToProviders: writeToProviders
            );

        return builder;
    }
}
