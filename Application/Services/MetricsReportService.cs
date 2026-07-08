using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class MetricsReportService
    {
        private readonly IMicrochannelRepository _repository;

        private readonly IEmailService _emailService;

        public MetricsReportService(IMicrochannelRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task GenerateAndSendDailyReportAsync()
        {
            var allContainers = await _repository.GetAllAsync();

            var inMesa = allContainers.Where(c => c.Status == "EN MESA" || c.Status == "ADENTRO").ToList();
            var outMesa = allContainers.Where(c => c.Status == "FUERA DE MESA" || c.Status == "AFUERA").ToList();

            int graysIn = inMesa.Count(c => c.Code.StartsWith("CONT-"));
            int orangesIn = inMesa.Count(c => c.Code.StartsWith("CTNA-"));
            int bluesIn = inMesa.Count(c => c.Code.StartsWith("CTAZ-"));
            int totalIn = inMesa.Count;

            int graysOut = outMesa.Count(c => c.Code.StartsWith("CONT-"));
            int orangesOut = outMesa.Count(c => c.Code.StartsWith("CTNA-"));
            int bluesOut = outMesa.Count(c => c.Code.StartsWith("CTAZ-"));
            int totalOut = outMesa.Count;

            int graysTotal = graysIn + graysOut;
            int orangesTotal = orangesIn + orangesOut;
            int bluesTotal = bluesIn + bluesOut;
            int grandTotal = allContainers.Count;

            string htmlTemplate = $@"
            <div style='font-family: Arial, sans-serif; max-width: 650px; margin: auto; border: 1px solid #e2e8f0; border-radius: 8px; overflow: hidden;'>
                <div style='background-color: #1e293b; color: white; padding: 20px; text-align: center;'>
                    <h2 style='margin: 0;'>Reporte Diario de Inventario</h2>
                    <p style='margin: 5px 0 0 0; color: #94a3b8;'>Distribución Global de Contenedores</p>
                </div>
                <div style='padding: 30px; background-color: #f8fafc;'>
                    <p style='color: #334155; font-size: 16px;'>Este es el estatus general del flujo de contenedores al corte de las 8:00 a.m.</p>
                    
                    <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                        <tr style='background-color: #e2e8f0;'>
                            <th style='padding: 12px; text-align: left; color: #475569;'>Tipo de Contenedor</th>
                            <th style='padding: 12px; text-align: right; color: #475569;'>EN MESA</th>
                            <th style='padding: 12px; text-align: right; color: #475569;'>FUERA</th>
                            <th style='padding: 12px; text-align: right; color: #475569;'>TOTAL</th>
                        </tr>
                        
                        <tr style='border-bottom: 1px solid #cbd5e1;'>
                            <td style='padding: 12px; font-weight: bold; color: #475569;'>Grises</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #10b981;'>{graysIn}</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #f97316;'>{graysOut}</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #64748b;'>{graysTotal}</td>
                        </tr>
                        
                        <tr style='border-bottom: 1px solid #cbd5e1;'>
                            <td style='padding: 12px; font-weight: bold; color: #f97316;'>Naranjas</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #10b981;'>{orangesIn}</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #f97316;'>{orangesOut}</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #64748b;'>{orangesTotal}</td>
                        </tr>
                        
                        <tr style='border-bottom: 1px solid #cbd5e1;'>
                            <td style='padding: 12px; font-weight: bold; color: #3b82f6;'>Azules</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #10b981;'>{bluesIn}</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #f97316;'>{bluesOut}</td>
                            <td style='padding: 12px; text-align: right; font-weight: bold; color: #64748b;'>{bluesTotal}</td>
                        </tr>
                        
                        <tr style='background-color: #f1f5f9;'>
                            <td style='padding: 12px; font-weight: 900; color: #0f172a;'>TOTAL GLOBAL</td>
                            <td style='padding: 12px; text-align: right; font-weight: 900; color: #10b981; font-size: 16px;'>{totalIn}</td>
                            <td style='padding: 12px; text-align: right; font-weight: 900; color: #f97316; font-size: 16px;'>{totalOut}</td>
                            <td style='padding: 12px; text-align: right; font-weight: 900; color: #0f172a; font-size: 18px;'>{grandTotal}</td>
                        </tr>
                    </table>
                </div>
            </div>";

            var emails = new List<string>()
            {
                "sebastian.gamez@mesa.ms",
                "anwar.delosmonteros@mesa.ms",
                "rolando.montelongo@mesa.ms",
                "jose.lugo@mesa.ms"
            };

            await _emailService.SendGlobalNotificationAsync(
                $"Reporte de Inventario de Contenedores - {DateTime.Now:dd/MM/yyyy}",
                htmlTemplate,
                emails
            );
        }
    }
}