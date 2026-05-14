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
    public class FtnInventoryRepository : IFtnInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public FtnInventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateFtnRecordAsync(FtnInventory ftnRecord)
        {
            await _context.FtnInventory.AddAsync(ftnRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FtnInventory>> GetActiveFtnRecordsByLineAsync(int lineId)
        {
            return await _context.FtnInventory
                    .Where(f => f.LineId == lineId && f.Status == "EN_TRANSITO")
                    .ToListAsync();
        }

        public async Task BulkUpdateFtnRecordsAsync(IEnumerable<FtnInventory> ftnRecords)
        {
            _context.FtnInventory.UpdateRange(ftnRecords);
            await _context.SaveChangesAsync();
        }
    }
}