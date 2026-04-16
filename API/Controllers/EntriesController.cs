using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly EntryService _entryService;

        public EntriesController(EntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [Route("history/{lineId}")]
        public async Task<IActionResult> GetHistory(int lineId)
        {
            var history = await _entryService.GetEntriesHistoryAsync(lineId);

            return Ok(history);
        }

        [HttpPut]
        [Route("UpdateEntry/{id}")]
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] EntryUpdateDto dto)
        {
            if(id != dto.Id)
            {
                return BadRequest(new
                {
                    message = "El ID de la URL no coincide con la petición"
                });
            }

            var success = await _entryService.UpdateEntryAsync(dto);
            if (!success)
            {
                return NotFound(new
                {
                    message = "No se encontró el registro de entrada para actualizar"
                });
            }

            return Ok(new
            {
                message = "Entrada actualizada correctamente"
            });
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(EntryCreateDto dto)
        {
            var id = await _entryService.RegisterEntryAsync(dto);

            return Ok(new
            {
                message = "Entrada registrada con éxit",
                id = id
            });
        }
    }
}