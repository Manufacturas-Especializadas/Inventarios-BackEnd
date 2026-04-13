using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class MovementService
    {
        private readonly IMovementQueries _queries;

        public MovementService(IMovementQueries queries)
        {
            _queries = queries;
        }

        public async Task<List<MovementDto>> GetLineMovementsAsync(int lineId)
        {
            return await _queries.GetMovementsByLineAsync(lineId);
        }
    }
}