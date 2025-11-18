using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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

            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("         TestingWindowsService v0.3.4");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine($"Iniciado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Directorio: {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"Ambiente: {(args.Contains("--development") ? "Development" : "Production")}");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

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
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
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
            _logger.LogInformation("╔════════════════════════════════════════════════════════════════╗");
            _logger.LogInformation("║ Servicio de Monitoreo Iniciado Correctamente                    ║");
            _logger.LogInformation("║ Logs guardados en: C:\\ProgramData\\TestingWindowsService\\       ║");
            _logger.LogInformation("║ Intervalo de monitoreo: 30 segundos                             ║");
            _logger.LogInformation("╚════════════════════════════════════════════════════════════════╝");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _eventCount++;
                    var timestamp = DateTime.Now;
                    var uptime = GC.GetTotalMemory(false) / 1024 / 1024; // MB
                    
                    _logger.LogInformation($"┌─ EVENTO #{_eventCount:D5} ─────────────────────────────────────────┐");
                    _logger.LogInformation($"│ Timestamp: {timestamp:yyyy-MM-dd HH:mm:ss}");
                    _logger.LogInformation($"│ Estado: Servicio monitorando activamente");
                    _logger.LogInformation($"│ Memoria: {uptime} MB");
                    _logger.LogInformation($"└─────────────────────────────────────────────────────────────────┘");
                    
                    await Task.Delay(30000, stoppingToken); // 30 segundos
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("╔════════════════════════════════════════════════════════════════╗");
                    _logger.LogInformation("║ Servicio detenido correctamente                                ║");
                    _logger.LogInformation($"║ Total de eventos procesados: {_eventCount}                                ║");
                    _logger.LogInformation("╚════════════════════════════════════════════════════════════════╝");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error en el servicio");
                }
            }
        }
    }
}
