using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FtnInventory
    {
        public int Id { get; set; }

        public int? ExitHeaderId { get; set; }

        public int? LineId { get; set; }

        public string Folio { get; set; } = string.Empty;

        public string? ShopOrder { get; set; }

        public string PartNumber { get; set; } = string.Empty;

        public int OriginalQuantity { get; set; }

        public int CurrentQuantity { get; set; }

        public string Status { get; set; } = "EN_TRANSITO";

        public DateTime CreatedAt { get; set; }

        public virtual ExitHeader? ExitHeader { get; set; }

        public virtual ProductionLine? ProductionLine { get; set; }
    }
}