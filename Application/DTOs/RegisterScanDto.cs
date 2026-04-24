using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegisterScanDto
    {
        public int ShippingReleaseId { get; set; }

        public string ScannedLabelId { get; set; } = string.Empty;
    }
}