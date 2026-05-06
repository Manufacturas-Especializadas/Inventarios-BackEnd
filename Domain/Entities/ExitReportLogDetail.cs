using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ExitReportLogDetail
    {
        public int Id { get; set; }

        public int ExitReportLogId { get; set; }

        public string Folio { get; set; } = string.Empty;

        public bool IsProcessed { get; set; } = false;

        public ExitReportLog ReportLog { get; set; } = null!;
    }
}