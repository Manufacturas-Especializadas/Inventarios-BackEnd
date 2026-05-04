using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExitRepository : IExitRepository
    {
        private readonly ApplicationDbContext _context;

        public ExitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateExitAsync(ExitHeader exit)
        {
            _context.ExitHeaders.Add(exit);
            await _context.SaveChangesAsync();

            return exit.Id;
        }

        public async Task<bool> UpdateExitAsync(ExitHeader exit)
        {
            var existingExit = await _context.ExitHeaders
                        .Include(e => e.Details)
                        .FirstOrDefaultAsync(e => e.Id == exit.Id);

            if (existingExit == null) return false;

            existingExit.ShopOrder1 = exit.ShopOrder1;
            existingExit.ShopOrder2 = exit.ShopOrder2;
            existingExit.ShopOrder3 = exit.ShopOrder3;
            existingExit.ShopOrder4 = exit.ShopOrder4;
            existingExit.ShopOrder5 = exit.ShopOrder5;

            _context.ExitDetails.RemoveRange(existingExit.Details);
            existingExit.Details = exit.Details;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteExitAsync(int id)
        {
            var exit = await _context.ExitHeaders
            .Include(e => e.Details)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (exit == null) return false;

            _context.ExitHeaders.Remove(exit);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsFolioProcessedAsync(string folio, int lineId)
        {
            return await _context.ExitHeaders
                        .AnyAsync(e => e.Folio == folio && e.LineId == lineId);
        }

        public async Task<IEnumerable<ExitHeader>> GetExitsHistoryByLineAsync(int lineId)
        {
            return await _context.ExitHeaders
                        .Include(e => e.Details)
                        .Where(e => e.LineId == lineId)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToListAsync();
        }

        public async Task<ExitHeader?> GetExitByIdAsync(int id)
        {
            return await _context.ExitHeaders
                    .Include(e => e.Details)
                    .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int?> GetStockByPartNumberAsync(string partNumber, int lineId)
        {
            bool hasEntries = await _context.EntryDetails
                .AnyAsync(d => d.PartNumber == partNumber && d.EntryHeader.LineId == lineId);

            if (!hasEntries)
            {
                return null;
            }

            var entries = await _context.EntryDetails
                .Where(d => d.PartNumber == partNumber && d.EntryHeader.LineId == lineId)
                .SumAsync(d => d.Quantity);

            var exits = await _context.ExitDetails
                .Where(d => d.PartNumber == partNumber && d.ExitHeader.LineId == lineId)
                .SumAsync(d => d.Quantity);

            return entries - exits;
        }
    
        public async Task<EntryHeader?> GetEntryByFolioAsync(string folio, int lineId)
        {
            return await _context.EntryHeaders
                    .Include(e => e.Details)
                    .FirstOrDefaultAsync(e => e.Folio == folio && e.LineId == lineId);
        }

        public async Task<IEnumerable<ExitHeader>> GetExitsByFolioAsync(List<string> folios)
        {
            var ids = folios
                .Where(f => int.TryParse(f, out _))
                .Select(int.Parse)
                .ToList();

            return await _context.ExitHeaders
                .Include(e => e.Details)
                .Where(e => folios.Contains(e.Folio) || ids.Contains(e.Id))
                .ToListAsync();
        }
    }
}