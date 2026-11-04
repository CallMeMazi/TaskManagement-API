using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Common.Helpers;
using TaskManagement.Common.Settings;
using TaskManagement.WebConfig.Logging;

namespace TaskManagement.WebConfig.Registeration;

public static class LoggingRegisteration
{
    // support in .Net 10
    extension(IHostBuilder hostBuilder)
    {
        public void RegisterSerilog(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IRequestTrace, HttpRequestTrace>();
            services.AddSingleton<RequestTraceEnricher>();

            EnableSelfLog();
            hostBuilder.UseSerilog((context, services, configuration) =>
                Configure(configuration, context, services)
            );
        }
    }
    
    private static void EnableSelfLog()
    {
        try
        {
            Directory.CreateDirectory("logs");
            var writer = TextWriter.Synchronized(new StreamWriter(
                Path.Combine("logs", "serilog-selflog.txt"),
                append: true)
            {
                AutoFlush = true
            });
            SelfLog.Enable(writer);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Operational self-log file could not be opened: {ex.Message}");
            SelfLog.Enable(Console.Error);
        }
    }
    private static void Configure(LoggerConfiguration logger, HostBuilderContext context, IServiceProvider services)
    {
        var options = context.Configuration.GetSection(OperationalLoggingOptions.SectionName).Get<OperationalLoggingOptions>()
            ?? new OperationalLoggingOptions();
        options.Validate();

        logger.MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Application", "TaskManagement.API")
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .Enrich.With(services.GetRequiredService<RequestTraceEnricher>());

        if (context.HostingEnvironment.IsDevelopment())
            logger.WriteTo.Console();

        var connectionString = context.Configuration.GetConnectionString("ApplicationLogConnectionString");
        if (connectionString.IsNullParameter())
        {
            if (!context.HostingEnvironment.IsDevelopment())
                throw new InvalidOperationException($"Connection string ApplicationLogConnectionString is required outside Development so operational Warning and Error logs can be stored.");

            Console.Error.WriteLine($"Connection string ApplicationLogConnectionString is missing. Operational SQL logging is disabled in Development.");
        }
        else
        {
            // The SQL sink is already batched. The async wrapper keeps a full queue from blocking the request.
            // When the queue is full, events are dropped. Sink failures are reported through SelfLog, not back through ILogger.
            // Host shutdown disposes Serilog and flushes the queue. A database outage does not fail the API request.
            // Error events are also written to the fallback file so they remain diagnosable when SQL is unavailable.
            logger.WriteTo.Logger(sql => sql
                .MinimumLevel.Warning()
                .WriteTo.Async(sink =>
                    sink.MSSqlServer(
                        connectionString,
                        new MSSqlServerSinkOptions
                        {
                            TableName = "OperationalLogs",
                            AutoCreateSqlTable = false,
                            BatchPostingLimit = options.SqlBatchSize,
                            BatchPeriod = TimeSpan.FromSeconds(2)
                        },
                        columnOptions: CreateColumnOptions(),
                        restrictedToMinimumLevel: LogEventLevel.Warning
                    ),
                    bufferSize: options.QueueCapacity,
                    blockWhenFull: false
                )
            );
        }

        logger.WriteTo.Logger(fallback =>
            fallback.MinimumLevel.Error()
            .WriteTo.Async(sink =>
                sink.File(
                    options.FallbackFilePath,
                    rollingInterval: RollingInterval.Infinite,
                    retainedFileCountLimit: options.FallbackRetainedFileCount,
                    restrictedToMinimumLevel: LogEventLevel.Error
                ),
                bufferSize: Math.Min(options.QueueCapacity, 1000),
                blockWhenFull: false
            )
        );
    }
    private static ColumnOptions CreateColumnOptions()
    {
        var columnOptions = new ColumnOptions();
        columnOptions.TimeStamp.ConvertToUtc = true;
        columnOptions.TimeStamp.DataType = SqlDbType.DateTime2;
        columnOptions.Properties.DataType = SqlDbType.NVarChar;
        columnOptions.Properties.DataLength = -1;
        columnOptions.AdditionalColumns = new Collection<SqlColumn>
        {
            new SqlColumn
            {
                ColumnName = "TraceId",
                PropertyName = "TraceId",
                DataType = SqlDbType.NVarChar,
                DataLength = 64,
                AllowNull = true
            },
            new SqlColumn
            {
                ColumnName = "SourceContext",
                PropertyName = "SourceContext",
                DataType = SqlDbType.NVarChar,
                DataLength = 256,
                AllowNull = true
            }
        };

        return columnOptions;
    }
}
