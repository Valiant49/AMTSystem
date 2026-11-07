namespace AMTSystem.Core.Models;

public class AssetDraft
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public int CategoryId { get; set; }
    public string SerialNumber { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
    public int ConditionId { get; set; }
    public string Components { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }

    public void Normalize()
    {
        Name = Name.Trim();
        SerialNumber = SerialNumber.Trim();
        Manufacturer = Manufacturer.Trim();
        Model = Model.Trim();
    }

    public List<string> Validate()
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Name is required.");

        if (string.IsNullOrWhiteSpace(SerialNumber))
            errors.Add("Serial number is required.");

        if (CategoryId == 0)
            errors.Add("Select a category.");

        if (ConditionId == 0)
            errors.Add("Select a condition.");

        if (string.IsNullOrWhiteSpace(Manufacturer))
            errors.Add("Manufacturer is required.");

        if (string.IsNullOrWhiteSpace(Model))
            errors.Add("Model is required");

        if (Name.Length > 100)
            errors.Add("Name must be 100 characters or fewer.");

        return errors;
    }


}
