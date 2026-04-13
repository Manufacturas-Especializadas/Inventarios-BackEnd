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
    }
}