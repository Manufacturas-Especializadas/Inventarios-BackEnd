using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EntryDetail
    {
        public int Id { get; set; }

        public int EntryHeaderId { get; set; }

        public string PartNumber { get; set; } = string.Empty;

        public string? Client { get; set; }

        public int Quantity { get; set; }

        public int? BoxesQuantity { get; set; }

        public EntryHeader EntryHeader{ get; set; } = null!;
    }
}