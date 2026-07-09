namespace Application.DTOs
{
    public class MicrochannelCreateDto
    {
        public string Code { get; set; }

        public string TypeMovement { get; set; }

        public int? TripNumber { get; set; }

        public int? PayRollNumber { get; set; }
    }
}