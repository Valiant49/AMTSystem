using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
using AMTSystem.Data;

namespace AMTSystem.Wpf.Views.AssetMgmt;
/// <summary>
/// Interaction logic for ReviewFormView.xaml
/// </summary>
public partial class ReviewFormView : UserControl
{
    public event EventHandler BackClicked;
    public event EventHandler SubmitClicked;

    public ReviewFormView(AssetDraft draft)
    {
        InitializeComponent();
        DataContext = draft;
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        BackClicked?.Invoke(this, EventArgs.Empty);
    }

    private void Submit_Click(object sender, RoutedEventArgs e)
    {
        SubmitClicked?.Invoke(this, EventArgs.Empty);
    }
}
