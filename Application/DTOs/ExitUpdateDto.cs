using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExitUpdateDto
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public string ShopOrder1 { get; set; } = string.Empty;

        public string? ShopOrder2 { get; set; }

        public string? ShopOrder3 { get; set; }

        public string? ShopOrder4 { get; set; }

        public string? ShopOrder5 { get; set; }

        public string? ShopOrder6 { get; set; }

        public List<ExitDetailDto> Details { get; set; } = new();
    }
}