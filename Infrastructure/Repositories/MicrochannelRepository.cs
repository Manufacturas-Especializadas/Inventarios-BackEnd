using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MicrochannelRepository : IMicrochannelRepository
    {
        private readonly ApplicationDbContext _context;

        public MicrochannelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MicrochannelInventory>> GetAllAsync()
        {
            return await _context.MicrochannelInventories
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<List<MicrochannelInventory>> GetRecentMovementAsync()
        {
            return await _context.MicrochannelInventories
                    .OrderByDescending(m => m.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<MicrochannelInventory?> GetOpenCycleAsync(string code)
        {
            return await _context.MicrochannelInventories
                .FirstOrDefaultAsync(m => m.Code == code && m.Status == "EN MESA");
        }

        public async Task<MicrochannelInventory> AddMovementAsync(MicrochannelInventory movement)
        {
            await _context.MicrochannelInventories.AddAsync(movement);

            await _context.SaveChangesAsync();

            return movement;
        }

        public async Task UpdateMovementAsync(MicrochannelInventory movement)
        {
            _context.MicrochannelInventories.Update(movement);
            await _context.SaveChangesAsync();
        }
    }
}