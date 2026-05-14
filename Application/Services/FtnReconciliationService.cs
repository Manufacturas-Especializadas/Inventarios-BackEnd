using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FtnReconciliationService
    {
        private readonly IFtnInventoryRepository _ftnRepository;

        public FtnReconciliationService(IFtnInventoryRepository ftnRepository)
        {
            _ftnRepository = ftnRepository;
        }

        public async Task<ReconciliationResultDto> ReconcileFromUploadedFileAsync(List<string> foliosFromExcel, int lineId)
        {
            var result = new ReconciliationResultDto();

            var activeFtnRecords = await _ftnRepository.GetActiveFtnRecordsByLineAsync(lineId);

            var recordsToUpdate = new List<FtnInventory>();

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            foreach(var scannedFolio in foliosFromExcel)
            {
                var cleanFolio = scannedFolio?.Trim();

                var matchingRecord = activeFtnRecords.FirstOrDefault(f => f.Folio == cleanFolio);

                if(matchingRecord != null)
                {
                    matchingRecord.CurrentQuantity = 0;
                    matchingRecord.Status = "LIQUIDADO";
                    matchingRecord.ClearedAt = nowInMexico;

                    recordsToUpdate.Add(matchingRecord);
                    result.SuccessfulFolios.Add(cleanFolio!);
                }
                else
                {
                    result.NotFoundOrAlreadyClearedFolios.Add(cleanFolio!);
                }
            }

            if (recordsToUpdate.Any())
            {
                await _ftnRepository.BulkUpdateFtnRecordsAsync(recordsToUpdate);
                result.TotalProcessed = recordsToUpdate.Count;
            }

           return result;
        }
    }
}