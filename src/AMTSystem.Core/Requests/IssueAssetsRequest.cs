using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Requests;

public enum TransactionType
{
    Lent, 
    Issued
}

public class IssueItem
{
    public int AssetId { get; set; }
    public int ConditionId {  get; set; }
    public string? Remarks { get; set; }

}

public class IssueAssetsRequest
{
    public int UserId { get; set; }
    public int PersonnelId { get; set; }
    public TransactionType Type { get; set; }
    public string? Remarks { get; set; }
    public List<IssueItem> Items { get; set; } = new();

    public List<string> Validate()
    {
        var errors = new List<string>();

        if (UserId <= 0)
            errors.Add("No logged-in users found.");

        if (PersonnelId <= 0)
            errors.Add("Select a personnel.");

        //if (Remarks?.Length > 500)
        //    errors.Add("Remarks must be 500 characters or fewer.");


        if (Items == null || Items.Count == 0)
        {
            errors.Add("Add at least one asset.");
        }
        else
        {
            if (Items.Any(i => i.AssetId <= 0))
                errors.Add("One or more assets are invalid.");

            if (Items.GroupBy(i => i.AssetId).Any(g => g.Count() > 1))
                errors.Add("Any asset appears more than once.");

            if (Items.Any(i => i.ConditionId <= 0))
                errors.Add("Every asset needs a condition");
        }

        return errors;
    }
}
