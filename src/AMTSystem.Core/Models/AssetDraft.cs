namespace AMTSystem.Core.Models;

public class AssetDraft
{
    public string Name {  get; set; }
    public int CategoryId { get; set; }
    public string SerialNumber { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
    public int ConditionId { get; set; }
    public string Components { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
}
