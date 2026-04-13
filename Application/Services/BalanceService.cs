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

        public async Task<List<PartBalanceDto>> GetLineBalancesAsync(int lineId)
        {
            return await _queries.GetLineBalancesAsync(lineId);
        }
    }
}