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
        [Route("history/{lineId}")]
        public async Task<IActionResult> GetHistory(int lineId)
        {
            var history = await _exitService.GetExitsHistoryAsync(lineId);

            return Ok(history);
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

        [HttpGet]
        [Route("preview/{lineId}/{folio}")]
        public async Task<IActionResult> GetFolioPreview(int lineId, string folio)
        {
            try
            {
                var preview = await _exitService.GetFolioPreviewAsync(folio, lineId);

                return Ok(preview);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("CreateExit")]
        public async Task<IActionResult> Create(ExitCreateDto dto)
        {
            try
            {
                var id = await _exitService.RegisterExitAsync(dto);
                return CreatedAtAction(
                    nameof(GetHistory), 
                    new { lineId = dto.LineId }, 
                    new { Id = id }
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("CreateExitByFolio")]
        public async Task<IActionResult> CreateExitByFolio([FromBody] ExitByFolioDto dto)
        {
            try
            {
                var id = await _exitService.RegisterExitByAsync(dto);

                return Ok(new
                {
                    message = $"Salida registrada correctamente para el folio {dto.Folio}",
                    exitId = id
                });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrio un error interno al precesar la salida"
                });
            }
        }

        [HttpPost]
        [Route("Generate-Report")]
        public async Task<ActionResult<IEnumerable<ExitReportDto>>> GetReport(int lineId, [FromBody] List<string> folios)
        {
            var result = await _exitService.GetReportDataAsync(lineId,folios);

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateExit/{id}")]
        public async Task<IActionResult> UpdateExit(int id, [FromBody] ExitUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest(new { Message = "El ID no coincide." });

            try
            {
                var success = await _exitService.UpdateExitAsync(dto);

                if (!success)
                {
                    return NotFound(new
                    {
                        Message = "No se encontró el registro."
                    });
                }

                return Ok(new
                {
                    Message = "Salida actualizada correctamente."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
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