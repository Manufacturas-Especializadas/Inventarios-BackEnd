using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EntryHistoryDto
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<EntryDetailDto> Details { get; set; } = new();

    }
}