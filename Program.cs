using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestingWindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<WindowsServiceWorker>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddFile();
                })
                .Build();

            host.Run();
        }
    }

    static class LoggingExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
        {
            return builder.AddProvider(new FileLoggerProvider());
        }
    }

    class FileLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName);
        public void Dispose() { }
    }

    class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private static readonly string _logDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TestingWindowsService"
        );
        private static readonly string _logFilePath = Path.Combine(_logDir, "service.log");

        public FileLogger(string categoryName)
        {
            _categoryName = categoryName;
            Directory.CreateDirectory(_logDir);
        }

        public IDisposable BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] {_categoryName} - {message}";

            if (exception != null)
                logEntry += $"\n{exception}";

            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch { }
        }
    }

    class WindowsServiceWorker : BackgroundService
    {
        private readonly ILogger<WindowsServiceWorker> _logger;
        private int _eventCount = 0;

        public WindowsServiceWorker(ILogger<WindowsServiceWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _eventCount++;
                    _logger.LogInformation($"[Evento #{_eventCount}] Servicio monitorando...");
                    await Task.Delay(30000, stoppingToken); // 30 segundos
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Servicio detenido");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el servicio");
                }
            }
        }
    }
}
