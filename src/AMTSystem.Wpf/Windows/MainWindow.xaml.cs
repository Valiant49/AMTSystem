using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core;
using AMTSystem.Core.Models;
using AMTSystem.Wpf.Views.AssetMgmt;
//using AMTSystem.Wpf.Views.AssetMgmt.EditAssetView;
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
        //AddDepartment.Visibility = Session.IsAdmin ? Visibility.Visible : Visibility.Collapsed;

        if (!Session.IsAdmin)
        {
            ManageDepartments.Visibility = Visibility.Collapsed;
        }

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
            case "EditAsset":
                ContentArea.Content = new EditAssetView();
                setHeaderText("Edit Asset");
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
            case "ManageDepartments":
                ManageDepartments dept_form = new ManageDepartments();
                dept_form.DeptSubmit_Clicked += Dept_Submit;
                ContentArea.Content = dept_form;
                setHeaderText("Manage Departments Record");
                break;
            // System Users
            case "SystemUsers":
                AllUsers all_users = new AllUsers();
                //line for some event handler thing
                ContentArea.Content = all_users;
                setHeaderText("All Users");
                break;
            case "AddUsers":
                AddUser add_user = new AddUser();
                //line for some event handler thing
                add_user.SubmitClick += User_Submit;
                ContentArea.Content = add_user;
                setHeaderText("Add System User");
                break;
            case "ManageUsers":
                ManageUsers manage_users = new ManageUsers();
                //line for some event handler thing
                ContentArea.Content = manage_users;
                setHeaderText("Manage Users");
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
        ContentArea.Content = new ManageDepartments();
        setHeaderText("Manage Departments Record");
    }

    private void Personnel_Submit(object? sender, EventArgs e)
    {
        ContentArea.Content = new AddPersonnel();
        setHeaderText("Add Personnel Record");
    }

    private void Manage_Submit(object? sender, EventArgs e)
    {
        ContentArea.Content = new ManageUsers();
        setHeaderText("Manage Users");
    }

    private void User_Submit(object? sender, EventArgs e)
    {
        ContentArea.Content = new AddUser();
        setHeaderText("Add System User");
    }
}