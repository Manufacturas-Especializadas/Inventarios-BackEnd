using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EntryUpdateDto
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public string ShopOrder { get; set; }

        public List<EntryDetailDto> Details { get; set; } = new();
    }
}