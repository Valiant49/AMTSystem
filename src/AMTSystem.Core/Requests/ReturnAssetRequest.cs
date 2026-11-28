namespace AMTSystem.Core.Requests;

public class ReturnItem
{
    public int TransactionId { get; set; }      // asset_transactions.id
    public int ConditionAtReturnId { get; set; }
    public string? Remarks { get; set; }
}

public class ReturnAssetsRequest
{
    public int UserId { get; set; }
    public List<ReturnItem> Items { get; set; } = new();

    public List<string> Validate()
    {
        var errors = new List<string>();

        if (UserId <= 0)
            errors.Add("No logged-in user found.");

        if (Items == null || Items.Count == 0)
        {
            errors.Add("Select at least one item to return.");
            return errors;
        }

        if (Items.Any(i => i.TransactionId <= 0))
            errors.Add("One or more items are invalid.");

        if (Items.GroupBy(i => i.TransactionId).Any(g => g.Count() > 1))
            errors.Add("An item appears more than once.");

        if (Items.Any(i => i.ConditionAtReturnId <= 0))
            errors.Add("Every returned item needs a condition.");

        return errors;
    }
}
