using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class BalanceQueries : IBalanceQueries
    {
        private readonly ApplicationDbContext _context;

        public BalanceQueries(ApplicationDbContext context) => _context = context;

        public async Task<List<PartBalanceDto>> GetLineBalancesAsync(int lineId)
        {
            var entriesSummary = await _context.EntryDetails
                        .Where(d => d.EntryHeader.LineId == lineId)
                        .GroupBy(d => new { d.PartNumber, d.Client })
                        .Select(g => new
                        {
                            PartNumber = g.Key.PartNumber,
                            Client = g.Key.Client,
                            TotalEntries = g.Sum(x => x.Quantity),
                            LastEntryDate = g.Max(x => x.EntryHeader.CreatedAt)
                        }).ToListAsync();

            var exitsRaw = await _context.ExitDetails
                        .Where(d => d.ExitHeader.LineId == lineId)
                        .Select(d => new {
                            d.PartNumber,
                            d.Quantity,
                            d.ExitHeader.CreatedAt,
                            d.ExitHeader.ShopOrder1,
                            d.ExitHeader.ShopOrder2,
                            d.ExitHeader.ShopOrder3,
                            d.ExitHeader.ShopOrder4,
                            d.ExitHeader.ShopOrder5,
                            d.ExitHeader.ShopOrder6
                        }).ToListAsync();

            var exitsSummary = exitsRaw
                        .GroupBy(d => d.PartNumber)
                        .Select(g => {
                            var allSOs = g.SelectMany(x => new[] { x.ShopOrder1, x.ShopOrder2, x.ShopOrder3, x.ShopOrder4, x.ShopOrder5, x.ShopOrder6 })
                                          .Where(so => !string.IsNullOrWhiteSpace(so))
                                          .Distinct();

                            return new
                            {
                                PartNumber = g.Key,
                                TotalExits = g.Sum(x => x.Quantity),
                                LastExitDate = g.Max(x => x.CreatedAt),
                                ShopOrders = string.Join(", ", allSOs) 
                            };
                        }).ToList();

            var balances = entriesSummary.Select(e =>
            {
                var exit = exitsSummary.FirstOrDefault(x => x.PartNumber == e.PartNumber);
                return new PartBalanceDto
                {
                    PartNumber = e.PartNumber,
                    Client = e.Client,
                    TotalEntries = e.TotalEntries,
                    TotalExits = exit?.TotalExits ?? 0,
                    Stock = e.TotalEntries - (exit?.TotalExits ?? 0),
                    LastEntryDate = e.LastEntryDate,
                    LastExitDate = exit?.LastExitDate,
                    ShopOrders = exit?.ShopOrders ?? "---"
                };
            }).OrderByDescending(b => b.LastEntryDate).ToList();

            return balances;
        }
    }
}