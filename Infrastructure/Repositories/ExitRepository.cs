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

    }
}