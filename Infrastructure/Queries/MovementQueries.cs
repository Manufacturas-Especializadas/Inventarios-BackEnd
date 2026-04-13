using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public class MovementQueries : IMovementQueries
    {
        private readonly ApplicationDbContext _context;

        public MovementQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovementDto>> GetMovementsByLineAsync(int lineId)
        {
            var entries = await _context.EntryDetails
                            .Where(d => d.EntryHeader.LineId == lineId)
                            .Select(d => new MovementDto
                            {
                                Id = d.Id,
                                Date = d.EntryHeader.CreatedAt,
                                Type = "ENTRADA",
                                PartNumber = d.PartNumber,
                                Client = d.Client,
                                Quantity = d.Quantity,
                                Reference = "INGRESO"
                            }).ToListAsync();

            var exits = await _context.ExitDetails
                            .Where(d => d.ExitHeader.LineId == lineId)
                            .Select(d => new MovementDto
                            {
                                Id = d.Id,
                                Date = d.ExitHeader.CreatedAt,
                                Type = "SALIDA",
                                PartNumber = d.PartNumber,
                                Client = d.Client,
                                Quantity = d.Quantity,
                                Reference = d.ExitHeader.ShopOrder1 +
                                            (d.ExitHeader.ShopOrder2 != null ? ", " + d.ExitHeader.ShopOrder2 : "") +
                                            (d.ExitHeader.ShopOrder3 != null ? ", " + d.ExitHeader.ShopOrder3 : "") +
                                            (d.ExitHeader.ShopOrder4 != null ? ", " + d.ExitHeader.ShopOrder4 : "") +
                                            (d.ExitHeader.ShopOrder5 != null ? ", " + d.ExitHeader.ShopOrder5 : "") +
                                            (d.ExitHeader.ShopOrder6 != null ? ", " + d.ExitHeader.ShopOrder6 : "")
                            }).ToListAsync();

            return entries.Concat(exits)
                            .OrderByDescending(m => m.Date)
                            .ToList();
        }

    }
}