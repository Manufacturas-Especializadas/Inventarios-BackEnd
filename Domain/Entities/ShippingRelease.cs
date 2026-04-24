using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingRelease
    {
        public int Id { get; set; }

        public string ShopOrder { get; set; } = null!;

        public string PartNumber { get; set; } = null!;

        public int TargetQuantity { get; set; }

        public string PackerName { get; set; } = null!;

        public int Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public ICollection<ShippingScan> Scans { get; set; } = new List<ShippingScan>();
    }
}