using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ExitService
    {
        private readonly IExitRepository _repository;
        private readonly IFtnInventoryRepository _ftnRepository;

        public ExitService(IExitRepository repository, 
            IFtnInventoryRepository ftnRepository)
        {
            _repository = repository;
            _ftnRepository = ftnRepository;
        }

        public async Task<int> RegisterExitAsync(ExitCreateDto dto)
        {
            foreach(var detail in dto.Details)
            {
                var currentStock = await _repository.GetStockByPartNumberAsync(detail.PartNumber, dto.LineId) ?? 0;

                if(detail.Quantity > currentStock)
                {
                    throw new InvalidOperationException($"Stock insuficiente para el No. Parte ${detail.PartNumber}. Tines {currentStock} y solicitates {detail.Quantity}");
                }
            }

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            var exit = new ExitHeader
            {
                LineId = dto.LineId,
                ShopOrder1 = dto.ShopOrder1,
                ShopOrder2 = dto.ShopOrder2,
                ShopOrder3 = dto.ShopOrder3,
                ShopOrder4 = dto.ShopOrder4,
                ShopOrder5 = dto.ShopOrder5,
                ShopOrder6 = dto.ShopOrder6,
                CreatedAt = nowInMexico,
                Details = dto.Details.Select(d => new ExitDetail
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity
                }).ToList()
            };

            return await _repository.CreateExitAsync(exit);
        }

        public async Task<int> RegisterExitByAsync(ExitByFolioDto dto)
        {
            bool isProcessed = await _repository.IsFolioProcessedAsync(dto.Folio, dto.LineId);

            if (isProcessed)
            {
                throw new InvalidOperationException("Este folio ya fue procesado y se le dio salida anteriormente");
            }

            var entryOriginal = await _repository.GetEntryByFolioAsync(dto.Folio, dto.LineId);

            if (entryOriginal == null)
            {
                throw new KeyNotFoundException("El folio escaneado no existe o no pertenece a esta línea");
            }

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            var exit = new ExitHeader
            {
                LineId = dto.LineId,
                Folio = dto.Folio,
                ShopOrder1 = string.IsNullOrEmpty(entryOriginal.ShopOrder) ? "N/A" : entryOriginal.ShopOrder,
                CreatedAt = nowInMexico,
                Details = entryOriginal.Details.Select(d => new ExitDetail
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity,
                }).ToList()
            };

            await _repository.MarkFolioAsProcessedInLogAsync(dto.Folio);

            int exitId = await _repository.CreateExitAsync(exit);

            if(dto.LineId == 11)
            {
                var mainPart = entryOriginal.Details.FirstOrDefault();
                var totalQty = entryOriginal.Details.Sum(d => d.Quantity);

                var ftnRecord = new FtnInventory
                {
                    ExitHeaderId = exitId,
                    LineId = dto.LineId,
                    Folio = dto.Folio,
                    ShopOrder = exit.ShopOrder1,
                    PartNumber = mainPart?.PartNumber ?? "N/A",
                    OriginalQuantity = totalQty,
                    CurrentQuantity = totalQty,
                    Status = "EN_TRANSITO",
                    CreatedAt = nowInMexico,
                };

                await _ftnRepository.CreateFtnRecordAsync(ftnRecord);
            }

            return exitId;
        }

        public async Task<bool> UpdateExitAsync(ExitUpdateDto dto)
        {
            var exitingExit = await _repository.GetExitByIdAsync(dto.Id);
            if (exitingExit == null) return false;
            
            foreach(var detail in dto.Details)
            {
                var currentStock = await _repository.GetStockByPartNumberAsync(detail.PartNumber, dto.LineId) ?? 0;
                var previousQuantity = exitingExit.Details.FirstOrDefault(d => d.PartNumber == detail.PartNumber)?.Quantity ?? 0;

                var realAvailableStock = currentStock + previousQuantity;

                if(detail.Quantity > realAvailableStock)
                {
                    throw new InvalidOperationException($"Stock insuficiente para {detail.PartNumber}. Máximo disponible: {realAvailableStock}");
                }
            }

            var exit = new ExitHeader
            {
                Id = dto.Id,
                LineId = dto.LineId,
                ShopOrder1 = dto.ShopOrder1,
                ShopOrder2 = dto.ShopOrder2,
                ShopOrder3 = dto.ShopOrder3,
                ShopOrder4 = dto.ShopOrder4,
                ShopOrder5 = dto.ShopOrder5,
                ShopOrder6 = dto.ShopOrder6,
                Details = dto.Details.Select(d => new ExitDetail
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity
                }).ToList()
            };

            return await _repository.UpdateExitAsync(exit);
        }

        public async Task<bool> DeleteExitAsync(int id)
        {
            return await _repository.DeleteExitAsync(id);
        }

        public async Task<int?> GetCurrentStock(string partNumber, int lineId)
        {
            return await _repository.GetStockByPartNumberAsync(partNumber, lineId);
        }

        public async Task<List<ExitHistoryDto>> GetExitsHistoryAsync(int lineId)
        {
            var exits = await _repository.GetExitsHistoryByLineAsync(lineId);

            return exits.Select(e => new ExitHistoryDto
            {
                Id = e.Id,
                LineId = e.LineId,
                ShopOrder1 = e.ShopOrder1,
                ShopOrder2 = e.ShopOrder2,
                ShopOrder3 = e.ShopOrder3,
                ShopOrder4 = e.ShopOrder4,
                ShopOrder5 = e.ShopOrder5,
                ShopOrder6 = e.ShopOrder6,
                CreatedAt = e.CreatedAt,
                Details = e.Details.Select(d => new ExitDetailDto
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity!.Value
                }).ToList()
            }).ToList();
        }

        public async Task<object> GetFolioPreviewAsync(string folio, int lineId)
        {
            if(await _repository.IsFolioProcessedAsync(folio, lineId))
            {
                throw new InvalidOperationException("Este folio ya tiene salida");
            }

            var entry = await _repository.GetEntryByFolioAsync(folio, lineId);
            if(entry == null)
            {
                throw new KeyNotFoundException("Folio no encontrado");
            }

            return new
            {
                ShopOrder = string.IsNullOrEmpty(entry.ShopOrder) ? "N/A" : entry.ShopOrder,
                PartNumber = string.Join(", ", entry.Details.Select(d => d.PartNumber)),
                Boxes = entry.Details.Sum(d => d.BoxesQuantity ?? 0),
                Pieces = entry.Details.Sum(d => d.Quantity)
            };
        }

        public async Task<IEnumerable<ExitReportDto>> GetReportDataAsync(int lineId,List<string> folios)
        {
            await _repository.CreateReportLogAsync(lineId, folios);

            var entities = await _repository.GetEntriesByFoliosAsync(folios);

            return entities.Select(e => new ExitReportDto
            {
                Folio = !string.IsNullOrEmpty(e.Folio) ? e.Folio : e.Id.ToString(),
                ShopOrder = e.ShopOrder ?? "N/A",
                PartNumber = e.Details.FirstOrDefault()?.PartNumber ?? "",
                Quantity = e.Details?.FirstOrDefault()?.Quantity ?? 0
            });
        }
        public async Task<IEnumerable<ExitReportLog>> GetReportLogsAsync(int lineId)
        {
            return await _repository.GetReportLogsAsync(lineId);
        }
    }
}