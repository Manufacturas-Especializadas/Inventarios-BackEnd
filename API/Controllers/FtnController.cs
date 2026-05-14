using Application.Interfaces;
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
        private readonly IExcelReaderService _excelReaderService;

        public FtnController(FtnReconciliationService reconciliationService,
            IFtnInventoryRepository ftnRepository, IExcelReaderService excelReaderService)
        {
            _reconciliationService = reconciliationService;
            _ftnRepository = ftnRepository;
            _excelReaderService = excelReaderService;
        }

        [HttpGet]
        [Route("balance/{lineId}")]
        public async Task<IActionResult> GetFtnBalance(int lineId)
        {
            var records = await _ftnRepository.GetAllFtnRecordsByLineAsync(lineId);

            return Ok(records);
        }

        [HttpPost]
        [Route("reconcile/{lineId}")]
        public async Task<IActionResult> Reconcile(int lineId, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Archivo no proporcionado o vacio");

            List<string> foliosFromExcel;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                foliosFromExcel = _excelReaderService.ExtractFoliosFromStream(stream);
            }

            if(foliosFromExcel.Count == 0)
            {
                return BadRequest("No se encontraron folios en la primera columna del archivo");
            }

            var result = await _reconciliationService.ReconcileFromUploadedFileAsync(foliosFromExcel, lineId);

            return Ok(result);
        }
    }
}