using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMicrochannelRepository
    {
        Task<List<MicrochannelInventory>> GetAllAsync();

        Task<List<MicrochannelInventory>> GetRecentMovementAsync();

        Task<MicrochannelInventory?> GetOpenCycleAsync(string code);

        Task<MicrochannelInventory> AddMovementAsync(MicrochannelInventory movement);

        Task UpdateMovementAsync(MicrochannelInventory movement);
    }
}