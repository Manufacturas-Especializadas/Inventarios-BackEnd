using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExitReportDto
    {
        public string Folio { get; set; }

        public string ShopOrder { get; set; }

        public string PartNumber { get; set; }

        public int Quantity { get; set; }
    }
}