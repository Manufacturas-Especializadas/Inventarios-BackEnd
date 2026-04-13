using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EntryCreateDto
    {
        public int LineId { get; set; }

        public List<EntryDetailDto> Details { get; set; } = new();
    }

    public class EntryDetailDto
    {
        public string PartNumber { get; set; } = string.Empty;
        public string? Client { get; set; }
        public int Quantity { get; set; }
    }
}