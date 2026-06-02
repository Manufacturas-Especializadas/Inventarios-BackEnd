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

        public string? ShopOrder { get; set; }

        public string? ShopOrder2 { get; set; }

        public string? ShopOrder3 { get; set; }

        public string? ShopOrder4 { get; set; }

        public string? ShopOrder5 { get; set; }

        public string? ShopOrder6 { get; set; }

        public List<EntryDetailDto> Details { get; set; } = new();
    }

    public class EntryDetailDto
    {
        public string PartNumber { get; set; } = string.Empty;
        public string? Client { get; set; }
        public int Quantity { get; set; }

        public int? BoxesQuantity { get; set; }
    }
}