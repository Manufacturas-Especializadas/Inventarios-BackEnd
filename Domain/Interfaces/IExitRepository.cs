using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExitRepository
    {
        Task<int> CreateExitAsync(ExitHeader exit);

        Task<int?> GetStockByPartNumberAsync(string partNumber, int lineId);

        Task<ExitHeader> GetExitByIdAsync(int id);

        Task<bool> UpdateExitAsync(ExitHeader exit);

        Task<bool> DeleteExitAsync(int id);

        Task<IEnumerable<ExitHeader>> GetExitsHistoryByLineAsync(int lineId);

        Task<bool> IsFolioProcessedAsync(string Folio, int lineId);
    }
}