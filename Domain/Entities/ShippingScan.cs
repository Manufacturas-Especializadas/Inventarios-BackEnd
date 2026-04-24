using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingScan
    {
        public int Id { get; set; }

        public int ShippingReleaseId { get; set; }

        public string ScannedLabelId { get; set; } = null!;

        public DateTime? ScannedAt { get; set; }

        public ShippingRelease ShippingRelease { get; set; } = null!;
    }
}