using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ShippingReleaseDto
    {
        public int Id { get; set; }

        public string ShopOrder { get; set; } = string.Empty;

        public string PartNumber { get; set; } = string.Empty;

        public int TargetQuantity { get; set; }

        public int CurrentScans { get; set; }

        public string PackerName { get; set; } = string.Empty;

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}