using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Job
{
    public class DailyMetricsJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DailyMetricsJob> _logger;

        public DailyMetricsJob(IServiceScopeFactory scopeFactory, ILogger<DailyMetricsJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de Reporte Diario de Contenedores Inicializado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, targetTimeZone);

                var nextRun = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);

                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;
                _logger.LogInformation($"Siguiente reporte programado para el {nextRun:dd/MM/yyyy HH:mm:ss}. Esperando {delay.TotalHours:F2} horas.");

                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Ejecutando envío automático de reporte diario...");

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var reportService = scope.ServiceProvider.GetRequiredService<MetricsReportService>();
                            await reportService.GenerateAndSendDailyReportAsync();
                        }

                        _logger.LogInformation("Reporte diario enviado con éxito.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al procesar el envío automático del reporte diario.");
                    }
                }
            }
        }
    }
}