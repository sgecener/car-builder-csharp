namespace CarBuilderAPI.models;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public WheelsDto Wheel { get; set; }
    public TechnologyDto Technology { get; set; }
    public PaintColorDto Paint { get; set; }
    public InteriorDto Interior { get; set; }
    public decimal TotalPrice { get; set; }
}