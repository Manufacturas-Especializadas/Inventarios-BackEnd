using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BalanceService
    {
        private readonly IBalanceQueries _queries;

        public BalanceService(IBalanceQueries queries) => _queries = queries;

        public async Task<List<PartBalanceDto>> GetLineBalancesAsync(int lineId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _queries.GetLineBalancesAsync(lineId, startDate, endDate);
        }
    }
}