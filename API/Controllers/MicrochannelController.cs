using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicrochannelController : ControllerBase
    {
        private readonly MicrochannelService _microchannelService;
        private readonly MetricsReportService _reportService;

        public MicrochannelController(MicrochannelService microchannelService, MetricsReportService reportService)
        {
            _microchannelService = microchannelService;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("recent")]
        public async Task<IActionResult> GetRecentMovements()
        {
            try
            {
                var recentScans = await _microchannelService.GetRecentAsync();

                return Ok(recentScans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error al obtener los movimientos recientes",
                    Details = ex.Message
                });
            }

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterMovement([FromBody] MicrochannelCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new
                    {
                        Message = "Los datos del movimiento no pueden ser nul"
                    });

                var result = await _microchannelService.RegisterMovementAsync(dto);

                return Ok(new
                {
                    Message = "Movimiento registrado con éxito",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                });
            }
        }

        [HttpPost("trigger-daily-report")]
        public async Task<IActionResult> TriggerReport()
        {
            try
            {
                await _reportService.GenerateAndSendDailyReportAsync();

                return Ok(new { message = "Reporte diario forzado y enviado con éxito por correo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al forzar el reporte: {ex.Message}" });
            }
        }
    }
}