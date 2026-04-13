using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly BalanceService _balanceService;
        private readonly IExcelReportService _excelService; 

        public BalancesController(BalanceService balanceService, 
        IExcelReportService excelService)
        {
            _balanceService = balanceService;
            _excelService = excelService;
        }

        [HttpGet]
        [Route("line/{lineId}")]
        public async Task<IActionResult> GetBalances(int lineId)
        {
            try
            {
                var balances = await _balanceService.GetLineBalancesAsync(lineId);

                if(balances == null || !balances.Any())
                {
                    return Ok(new List<object>());
                }

                return Ok(balances);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno al calcular el balance de inventario",
                    details = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("export/line/{lineId}")]
        public async Task<IActionResult> ExportToExcel(int lineId, [FromQuery] string lineName = "")
        {
            try
            {
                var balances = await _balanceService.GetLineBalancesAsync(lineId);

                if (balances == null || !balances.Any())
                {
                    return BadRequest("No hay datos para exportar en esta línea.");
                }

                string finalLineName = string.IsNullOrWhiteSpace(lineName)
                    ? $"LINEA {lineId}"
                    : lineName;

                var excelBytes = _excelService.GenerateBalanceReport(balances, finalLineName);

                string safeFileName = finalLineName.Replace(" ", "_");

                return File(
                    fileContents: excelBytes,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"Reporte_Inventario_{safeFileName}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al generar el reporte", Details = ex.Message });
            }
        }
    }
}