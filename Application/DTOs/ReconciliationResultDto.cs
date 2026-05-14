using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReconciliationResultDto
    {
        public int TotalProcessed { get; set; }

        public List<string> SuccessfulFolios { get; set; } = new List<string>();

        public List<string> NotFoundOrAlreadyClearedFolios { get; set; } = new List<string>();
    }
}