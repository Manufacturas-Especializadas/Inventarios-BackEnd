using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EntryHeader
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<EntryDetail> Details { get; set; } = new();
    }
}