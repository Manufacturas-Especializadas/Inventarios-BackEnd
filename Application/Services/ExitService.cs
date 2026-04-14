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
    public class ExitService
    {
        private readonly IExitRepository _repository;

        public ExitService(IExitRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> RegisterExitAsync(ExitCreateDto dto)
        {
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

        public async Task<int?> GetCurrentStock(string partNumber, int lineId)
        {
            return await _repository.GetStockByPartNumberAsync(partNumber, lineId);
        }
    }
}