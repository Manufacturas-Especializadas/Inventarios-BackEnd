using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEntryRepository
    {
        Task<int> CreateEntryAsync(EntryHeader entry);

        Task<EntryHeader> GetEntryByIdAsync(int id);

        Task<bool> UpdateEntryAsync(EntryHeader entry);

        Task<bool> DeleteEntryAsync(int id);

        Task<IEnumerable<EntryHeader>> GetEntriesHistoryByLineAsync(int lineId);

        Task<int> GetDailyEntriesCountAsync(int lineId, DateTime date);

        Task<int> GetMaxFolioSequenceTodayAsync(int lineId, DateTime date);
    }
}