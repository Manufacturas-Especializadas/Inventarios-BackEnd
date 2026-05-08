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
    public class EntryRepository : IEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public EntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EntryHeader> GetEntryByIdAsync(int id)
        {
            return await _context.EntryHeaders
                    .Include(e => e.Details)
                    .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<EntryHeader>> GetEntriesHistoryByLineAsync(int lineId)
        {
            return await _context.EntryHeaders
                    .Include(e => e.Details)
                    .Where(e => e.LineId == lineId)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync();
        }

        public async Task<int> GetDailyEntriesCountAsync(int lineId, DateTime date)
        {
            return await _context.EntryHeaders
                    .Where(e => e.LineId == lineId && e.CreatedAt.Date == date.Date)
                    .CountAsync();
        }

        public async Task<int> GetMaxFolioSequenceAsync(int lineId)
        {
            var entries = await _context.EntryHeaders
                .Where(e => e.LineId == lineId)
                .Select(e => e.Folio)
                .ToListAsync();

            if (!entries.Any()) return 0;

            int maxSequence = 0;
            foreach (var folio in entries)
            {
                if (int.TryParse(folio, out int currentSequence))
                {
                    if (currentSequence > maxSequence)
                    {
                        maxSequence = currentSequence;
                    }
                }
            }

            return maxSequence;
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            var entry = await _context.EntryHeaders
            .Include(e => e.Details)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (entry == null) return false;

            _context.EntryHeaders.Remove(entry);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateEntryAsync(EntryHeader entry)
        {
            var existingEntry = await _context.EntryHeaders
                        .Include(e => e.Details)
                        .FirstOrDefaultAsync(e => e.Id == entry.Id);

            if (existingEntry == null) return false;

            _context.EntryDetails.RemoveRange(existingEntry.Details);
            existingEntry.Details = entry.Details;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> CreateEntryAsync(EntryHeader entry)
        {
            _context.EntryHeaders.Add(entry);
            await _context.SaveChangesAsync();

            return entry.Id;
        }
    }
}