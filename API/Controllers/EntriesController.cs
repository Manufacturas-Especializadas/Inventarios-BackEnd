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