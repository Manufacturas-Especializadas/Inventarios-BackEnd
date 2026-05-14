using Application.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FtnController : ControllerBase
    {
        private readonly FtnReconciliationService _reconciliationService;
        private readonly IFtnInventoryRepository _ftnRepository;

        public FtnController(FtnReconciliationService reconciliationService,
            IFtnInventoryRepository ftnRepository)
        {
            _reconciliationService = reconciliationService;
            _ftnRepository = ftnRepository;
        }

        [HttpGet]
        [Route("balance/{lineId}")]
        public async Task<IActionResult> GetFtnBalance(int lineId)
        {
            var records = await _ftnRepository.GetActiveFtnRecordsByLineAsync(lineId);

            return Ok(records);
        }

        [HttpPost]
        [Route("reconcile/{lineId}")]
        public async Task<IActionResult> Reconcile(int lineId, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Archivo no proprocionado");

            var foliosFromExcel = new List<string>();

            var result = await _reconciliationService.ReconcileFromUploadedFileAsync(foliosFromExcel, lineId);

            return Ok(result);
        }
    }
}