using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExitsController : ControllerBase
    {
        private readonly ExitService _exitService;

        public ExitsController(ExitService exitService)
        {
            _exitService = exitService;
        }

        [HttpGet]
        [Route("stock/{lineId}/{partNumber}")]
        public async Task<IActionResult> GetStock(int lineId, string partNumber)
        {
            var stock = await _exitService.GetCurrentStock(partNumber, lineId);

            if(stock == null)
            {
                return NotFound(new
                {
                    message = "Este número de parte aún no tiene registros de entrada"
                });
            }

            return Ok(new
            {
                partNumber = partNumber,
                stock = stock
            });
        }


        [HttpPost]
        [Route("CreateExit")]
        public async Task<IActionResult> Create(ExitCreateDto dto)
        {
            var id = await _exitService.RegisterExitAsync(dto);

            return Ok(new
            {
                message = "Salida registrada",
                id = id
            });
        }

        [HttpPut]
        [Route("UpdateExit/{id}")]
        public async Task<IActionResult> UpdateExit(int id, [FromBody] ExitUpdateDto dto)
        {
            if(id != dto.Id)
            {
                return BadRequest(new
                {
                    message = "El ID de la URL no coinicde con la petición"
                });                
            }

            var success = await _exitService.UpdateExitAsync(dto);

            if (!success)
            {
                return NotFound(new
                {
                    message = "No se encontró el registro de salida para actualizar"
                });
            }

            return Ok(new
            {
                message = "Salida actualizada correctamente"
            });
        }

        [HttpDelete]
        [Route("DeleteExit/{id}")]
        public async Task<IActionResult> DeleteExit(int id)
        {
            var success = await _exitService.DeleteExitAsync(id);

            if (!success)
            {
                return NotFound(new
                {
                    message = "No se encontró el registro de salida para eliminar"
                });
            }

            return Ok(new
            {
                message = "Salida eliminada correctamente"
            });
        }
    }
}