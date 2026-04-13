using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMovementQueries
    {
        Task<List<MovementDto>> GetMovementsByLineAsync(int lineId);
    }
}