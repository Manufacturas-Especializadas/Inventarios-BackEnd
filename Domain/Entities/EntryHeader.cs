using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EntryHeader
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? ShopOrder { get; set; }

        public string? ShopOrder2 { get; set; }

        public string? ShopOrder3 { get; set; }

        public string? ShopOrder4 { get; set; }

        public string? ShopOrder5 { get; set; }

        public string? ShopOrder6 { get; set; }

        public string? Folio { get; set; }

        public List<EntryDetail> Details { get; set; } = new();
    }
}