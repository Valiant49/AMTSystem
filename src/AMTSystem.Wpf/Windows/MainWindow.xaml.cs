using System.Windows;
using System.Windows.Controls;

using AMTSystem.Wpf.Views.AssetMgmt;

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

        switch (tag)
        {
            case "Dashboard":
                ContentArea.Content = new DashboardView();
                setHeaderText("Dashboard");
                break;
            case "AllAssets":
                ContentArea.Content = new AllAssetsView();
                setHeaderText("All Assets");
                break;
            case "Register":
                //ContentArea.Content = new RegisterView(new Core.Models.AssetDraft());
                ContentArea.Content = new RegisterFlowView();
                setHeaderText("Register Assets");
                break;
            case "Issue":
                ContentArea.Content = new IssueView();
                setHeaderText("Issue Assets");
                break;
            case "Lend":
                ContentArea.Content = new LendView();
                setHeaderText("Lend Assets");
                break;
            case "Return":
                ContentArea.Content = new ReturnView();
                setHeaderText("Return Assets");
                break;
        }
    }
}