using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ExitReportLog
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public DateTime PrintedAt { get; set; }

        public List<ExitReportLogDetail> Details { get; set; } = new List<ExitReportLogDetail>();
    }
}