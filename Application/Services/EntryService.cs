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

        public async Task<int> RegisterEntryAsync(EntryCreateDto dto)
        {
            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            var entry = new EntryHeader
            {
                LineId = dto.LineId,
                CreatedAt = nowInMexico,
                Details = dto.Details.Select(d => new EntryDetail
                {
                    PartNumber = d.PartNumber,
                    Client = d.Client,
                    Quantity = d.Quantity
                }).ToList()
            };

            return await _repository.CreateEntryAsync(entry);
        }
    }
}