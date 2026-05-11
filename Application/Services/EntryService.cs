using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EntryService
    {
        private readonly IEntryRepository _repository;

        public EntryService(IEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EntryHistoryDto>> GetEntriesHistoryAsync(int lineId)
        {
            var entries = await _repository.GetEntriesHistoryByLineAsync(lineId);

            return entries.Select(e => new EntryHistoryDto
            {
                Id = e.Id,
                LineId = e.LineId,
                CreatedAt = e.CreatedAt,
                Folio = e.Folio!,
                ShopOrder = e.ShopOrder!,
                Details = e.Details.Select(d => new EntryDetailDto
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity,
                    BoxesQuantity = d.BoxesQuantity,
                }).ToList()
            }).ToList();
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            return await _repository.DeleteEntryAsync(id);
        }

        public async Task<bool> UpdateEntryAsync(EntryUpdateDto dto)
        {
            var entry = new EntryHeader
            {
                Id = dto.Id,
                LineId = dto.LineId,
                ShopOrder = dto.ShopOrder,
                Details = dto.Details.Select(d => new EntryDetail
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity,
                    BoxesQuantity= d.BoxesQuantity,
                }).ToList()
            };

            return await _repository.UpdateEntryAsync(entry);
        }

        public async Task<string> RegisterEntryAsync(EntryCreateDto dto)
        {
            if (dto.LineId == 11)
            {
                if (string.IsNullOrEmpty(dto.ShopOrder))
                    throw new Exception("El Shop Order es obligatorio para la Línea 12");

                if (dto.Details.Any(d => d.BoxesQuantity == null || d.BoxesQuantity <= 0))
                    throw new Exception("La cantidad de cajas es obligatoria para la Línea 12");
            }

            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            string generatedFolio = string.Empty;

            if (dto.LineId == 11)
            {
                int maxSequence = await _repository.GetMaxFolioSequenceAsync(dto.LineId);

                string sequence = (maxSequence + 1).ToString("D5");

                generatedFolio = $"{sequence}";
            }

            var entry = new EntryHeader
            {
                LineId = dto.LineId,
                ShopOrder = dto.ShopOrder,
                Folio = generatedFolio,
                CreatedAt = nowInMexico,
                Details = dto.Details.Select(d => new EntryDetail
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity,
                    BoxesQuantity = d.BoxesQuantity,
                }).ToList()
            };

            await _repository.CreateEntryAsync(entry);

            return generatedFolio;
        }
    }
}