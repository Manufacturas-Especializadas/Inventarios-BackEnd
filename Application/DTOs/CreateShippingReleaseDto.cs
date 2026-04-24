using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateShippingReleaseDto
    {
        public string ShopOrder { get; set; } = string.Empty;

        public string PartNumber { get; set; } = string.Empty;

        public int TargetQuantity { get; set; }

        public string PackerName { get; set; } = string.Empty;
    }
}