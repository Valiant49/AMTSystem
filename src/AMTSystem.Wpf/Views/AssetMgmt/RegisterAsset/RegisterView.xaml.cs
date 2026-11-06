using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;

namespace AMTSystem.Wpf.Views.AssetMgmt;

/// <summary>
/// Interaction logic for RegisterView.xaml
/// </summary>
public partial class RegisterView : UserControl
{
    public event EventHandler NextClicked;

    public RegisterView(AssetDraft draft)
    {
        InitializeComponent();
        DataContext = draft;
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
        if (NextClicked != null)
        {
            NextClicked(this, EventArgs.Empty);
        }
    }
}
