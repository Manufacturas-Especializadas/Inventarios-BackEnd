using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExitRepository
    {
        Task<int> CreateExitAsync(ExitHeader exit);

        Task<int?> GetStockByPartNumberAsync(string partNumber, int lineId);
    }
}