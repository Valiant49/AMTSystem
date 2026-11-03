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

    public void setHeaderText(string headerText)
    {
        HeaderText.Text = headerText;
    }

    public void Nav_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Button)?.Tag as string;

        //ContentArea.Content = tag;
            
        switch (tag)
        {
            case "Dashboard":
                new DashboardView();
                setHeaderText("Dashboard");
                break;
            case "AllAssets":
                new AllAssetsView();
                setHeaderText("All Assets");
                break;
            case "Register":
                new RegisterView();
                setHeaderText("Register Assets");
                break;
            case "Issue":
                new IssueView();
                setHeaderText("Issue Assets");
                break;
            case "Lend":
                new LendView();
                setHeaderText("Lend Assets");
                break;
            case "Return":
                new ReturnView();
                setHeaderText("Return Assets");
                break;
        }
    }
}