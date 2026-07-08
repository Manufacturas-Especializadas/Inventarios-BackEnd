namespace Domain.Entities
{
    public class MicrochannelInventory
    {
        public int Id { get; set; }

        public string Company { get; set; } = "Manufacturas Especializadas";

        public string Area { get; set; } = "CONTENEDOR";

        public string Description { get; set; } = "CONTENEDOR MICROCHANNEL";

        public string Line { get; set; } = "Linea 6";

        public string Code { get; set; }

        public int? TripNumber { get; set; }

        public string Status { get; set; } = "EN MESA";

        public DateTime? CreatedAt { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ExitDate { get; set; }
    }
}