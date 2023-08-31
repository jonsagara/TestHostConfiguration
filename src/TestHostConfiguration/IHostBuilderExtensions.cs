using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TestHostConfiguration;

public static class IHostBuilderExtensions
{
    /// <summary>
    /// Sets Serilog as the logging provider.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    /// <param name="configureLogger"></param>
    /// <param name="preserveStaticLogger"></param>
    /// <param name="writeToProviders"></param>
    /// <returns></returns>
    public static ILoggingBuilder UseSerilog(this ILoggingBuilder builder, ConfigurationManager config, Action<IConfiguration, IServiceProvider, LoggerConfiguration> configureLogger, bool preserveStaticLogger = false, bool writeToProviders = false)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureLogger);

        IConfiguration config2 = config;
        builder.Services.AddSerilog(
            (serviceProvider, loggerConfiguration) =>
            {
                configureLogger(config2, serviceProvider, loggerConfiguration);
            },
            preserveStaticLogger: preserveStaticLogger,
            writeToProviders: writeToProviders
            );

        return builder;
    }

    ///// <summary>
    ///// Sets Serilog as the logging provider.
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <param name="config"></param>
    ///// <param name="configureLogger"></param>
    ///// <param name="preserveStaticLogger"></param>
    ///// <param name="writeToProviders"></param>
    ///// <returns></returns>
    //public static IHostBuilder UseSerilog(this IHostBuilder builder, ConfigurationManager config, Action<IConfiguration, IServiceProvider, LoggerConfiguration> configureLogger, bool preserveStaticLogger = false, bool writeToProviders = false)
    //{
    //    ArgumentNullException.ThrowIfNull(builder);
    //    ArgumentNullException.ThrowIfNull(configureLogger);

    //    builder.ConfigureServices(services =>
    //    {
    //        IConfiguration config2 = config;
    //        services.AddSerilog(
    //            (serviceProvider, loggerConfiguration) =>
    //                {
    //                    configureLogger(config2, serviceProvider, loggerConfiguration);
    //                },
    //            preserveStaticLogger: preserveStaticLogger,
    //            writeToProviders: writeToProviders
    //            );
    //    });

    //    return builder;
    //}
}

/*
//
        // Summary:
        //     Sets Serilog as the logging provider.
        //
        // Parameters:
        //   builder:
        //     The host builder to configure.
        //
        //   configureLogger:
        //     The delegate for configuring the Serilog.LoggerConfiguration that will be used
        //     to construct a Serilog.Core.Logger.
        //
        //   preserveStaticLogger:
        //     Indicates whether to preserve the value of Serilog.Log.Logger.
        //
        //   writeToProviders:
        //     By default, Serilog does not write events to Microsoft.Extensions.Logging.ILoggerProviders
        //     registered through the Microsoft.Extensions.Logging API. Normally, equivalent
        //     Serilog sinks are used in place of providers. Specify true to write events to
        //     all providers.
        //
        // Returns:
        //     The host builder.
        //
        // Remarks:
        //     A Microsoft.Extensions.Hosting.HostBuilderContext is supplied so that configuration
        //     and hosting information can be used. The logger will be shut down when application
        //     services are disposed.
        public static IHostBuilder UseSerilog(this IHostBuilder builder, Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogger, bool preserveStaticLogger = false, bool writeToProviders = false)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (configureLogger == null)
            {
                throw new ArgumentNullException("configureLogger");
            }

            builder.ConfigureServices(delegate (HostBuilderContext context, IServiceCollection collection)
            {
                HostBuilderContext context2 = context;
                collection.AddSerilog(delegate (IServiceProvider services, LoggerConfiguration loggerConfiguration)
                {
                    configureLogger(context2, services, loggerConfiguration);
                }, preserveStaticLogger, writeToProviders);
            });
            return builder;
        }
*/