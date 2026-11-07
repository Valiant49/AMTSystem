using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
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
                RegisterView form = new RegisterView(new AssetDraft());
                form.SubmitClicked += Register_Submitted;
                ContentArea.Content = form;
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

    private void Form_SubmitClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Register_Submitted(object? sender, EventArgs e)
    {
        ContentArea.Content = new AllAssetsView();
        setHeaderText("All Assets");
    }
}