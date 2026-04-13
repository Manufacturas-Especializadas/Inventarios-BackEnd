using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExitCreateDto
    {
        public int LineId { get; set; }

        public string ShopOrder1 { get; set; } = string.Empty;

        public string? ShopOrder2 { get; set; }

        public string? ShopOrder3 { get; set; }

        public string? ShopOrder4 { get; set; }

        public string? ShopOrder5 { get; set; }

        public string? ShopOrder6 { get; set; }

        public List<ExitDetailDto> Details { get; set; }
    }

    public class ExitDetailDto
    {
        public string PartNumber { get; set; }

        public string? Client { get; set; }

        public int Quantity { get; set; }
    }
}