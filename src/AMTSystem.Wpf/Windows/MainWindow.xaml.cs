using System.Windows;
using System.Windows.Controls;

using AMTSystem.Wpf.Views;

namespace AMTSystem.Wpf.Windows;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ContentArea.Content = new DashboardView();
    }

    public void Nav_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Button)?.Tag as string;

        ContentArea.Content = tag switch
        {
            //"Dashboard" => new DashboardView(),
            _ => new TextBlock { Text = $"{tag} (not built yet)" }
        };
    }
}