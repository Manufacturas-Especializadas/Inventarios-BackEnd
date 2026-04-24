using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet]
        [Route("Release/{id}")]
        public async Task<IActionResult> GetRelease(int id)
        {
            var release = await _shippingService.GetActiveReleaseAsync(id);

            if(release == null)
                return NotFound(new
                {
                    message = $"No se encontró una orden de liberación"
                });

            return Ok(release);
        }

        [HttpPost]
        [Route("Release")]
        public async Task<IActionResult> CreateRelease([FromBody] CreateShippingReleaseDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var release = await _shippingService.CreateReleaseAsync(dto);

                return Ok(release);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al crear la orden de liberación"
                });
            }
        }

        [HttpPost]
        [Route("Scan")]
        public async Task<IActionResult> RegisterScan([FromBody] RegisterScanDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _shippingService.RegisterScanAsync(dto);

                if (!success)
                {
                    return BadRequest(new
                    {
                        message = "No se pudo registrar el escaneo"
                    });
                }

                var updatedRelease = await _shippingService.GetActiveReleaseAsync(dto.ShippingReleaseId);

                return Ok(updatedRelease);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrio un error al registrar el escaneo"
                });
            }
        }
    }
}