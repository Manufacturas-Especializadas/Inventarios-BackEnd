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

        public async Task<EntryHeader?> GetEntryByIdAsync(int id)
        {
            return await _context.EntryHeaders
                    .Include(e => e.Details)
                    .FirstOrDefaultAsync(e => e.Id == id);
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