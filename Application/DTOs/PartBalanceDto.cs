using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PartBalanceDto
    {
        public string PartNumber { get; set; } = string.Empty;

        public string? Client { get; set; }

        public int TotalEntries { get; set; }

        public int TotalExits { get; set; }

        public int Stock { get; set; }

        public DateTime? LastEntryDate { get; set; }

        public DateTime? LastExitDate { get; set; }

        public string ShopOrders { get; set; } = string.Empty;
    }
}