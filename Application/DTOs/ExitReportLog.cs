using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExitReportLog
    {
        public int Id { get; set; }

        public int LineId { get; set; }
 
        public List<ExitReportLogDetail> Details { get; set; } = new List<ExitReportLogDetail>();
    }
}