using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AMTSystem.Core.Models;

public class HeldItem : INotifyPropertyChanged
{
    public int TransactionId { get; set; }
    public int BatchId { get; set; }
    public int AssetId { get; set; }
    public string Tag { get; set; } = "";
    public string Category_String { get; set; } = "";
    public string Model { get; set; } = "";
    public string TransactionType { get; set; } = "";
    public DateTime TransactionDate { get; set; }
    public int ConditionAtIssueId { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set { if (_isSelected != value) { _isSelected = value; OnChanged(); } }
    }

    private int _conditionAtReturnId;
    public int ConditionAtReturnId
    {
        get => _conditionAtReturnId;
        set { if (_conditionAtReturnId != value) { _conditionAtReturnId = value; OnChanged(); } }
    }

    private string? _returnRemarks;
    public string? ReturnRemarks
    {
        get => _returnRemarks;
        set { if (_returnRemarks != value) { _returnRemarks = value; OnChanged(); } }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
