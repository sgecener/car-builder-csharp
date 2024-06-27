namespace CarBuilderAPI.models;

public class OrderDto
{
    public int Id { get; set; }

    public int PaintColorId { get; set; }

    public int TechnologyId { get; set; }

    public int WheelId { get; set; }

    public int InteriorId { get; set; }

    public DateTime Timestamp { get; set; }
    public WheelsDto Wheel { get; set; }
    public TechnologyDto Technology { get; set; }
    public PaintColorDto Paint { get; set; }
    public InteriorDto Interior { get; set; }
    public decimal TotalPrice { get; set; }
}