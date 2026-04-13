namespace Domain.Entities;

public class ExitDetail
{
    public int Id { get; set; }

    public int ExitHeaderId { get; set; }

    public string PartNumber { get; set; } = string.Empty;

    public string? Client { get; set; }

    public int Quantity { get; set; }

    public ExitHeader ExitHeader { get; set; } = null!;
}