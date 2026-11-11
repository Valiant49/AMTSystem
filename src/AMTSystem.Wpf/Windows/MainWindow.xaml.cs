using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
using AMTSystem.Wpf.Views.AssetMgmt;
using AMTSystem.Wpf.Views.PersonnelMgmt;
using AMTSystem.Wpf.Views.PersonnelMgmt.Departments;
using AMTSystem.Wpf.Views.UserMgmt;

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
            // Asset Management
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
            // Personnel Management
            case "AllPersonnel":
                AllPersonnel allPersonnel = new AllPersonnel();
                ContentArea.Content = allPersonnel;
                setHeaderText("All Personnel Records");
                break;
            case "AddPersonnel":
                AddPersonnel addPersonnel = new AddPersonnel();
                addPersonnel.SubmitClicked += Personnel_Submit;
                ContentArea.Content = addPersonnel;
                setHeaderText("Add Personnel Record");
                break;
            case "EditPersonnel":
                ContentArea.Content = new EditPersonnel();
                setHeaderText("Edit Personnel Information");
                break;
            case "AddDepartment":
                AddDepartment dept_form = new AddDepartment();
                dept_form.DeptSubmit_Clicked += Dept_Submit;
                ContentArea.Content = dept_form;
                setHeaderText("Add Department Record");
                break;
            // System Users
            case "SystemUsers":
                AllUsers all_users = new AllUsers();
                //line for some event handler thing
                ContentArea.Content = all_users;
                setHeaderText("All Users");
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

    private void Dept_Submit(object? sender, EventArgs e)
    {
        ContentArea.Content = new AddDepartment();
        setHeaderText("Add Department Record");
    }

    private void Personnel_Submit(object? sender, EventArgs e)
    {
        ContentArea.Content = new AddPersonnel();
        setHeaderText("Add Personnel Record");
    }
}