using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class MovementDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; } = string.Empty;

        public string PartNumber { get; set; } = string.Empty;

        public string? Client { get; set; }

        public int Quantity { get; set; }

        public string Reference { get; set; } = string.Empty;

        public ExitHeader ExitHeader { get; set; } = new();
    }
}