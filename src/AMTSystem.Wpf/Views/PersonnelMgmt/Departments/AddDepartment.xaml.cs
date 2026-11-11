using System.Windows;
using System.Windows.Controls;

using AMTSystem.Data;

using MySqlConnector;

namespace AMTSystem.Wpf.Views.PersonnelMgmt.Departments;
/// <summary>
/// Interaction logic for AddDepartment.xaml
/// </summary>
public partial class AddDepartment : UserControl
{
    public event EventHandler? DeptSubmit_Clicked;

    public AddDepartment()
    {
        InitializeComponent();
        LoadDepartments();
    }

    private void Dept_Submit_Clicked(object sender, RoutedEventArgs e)
    {
        string department_input = DeptInput.Text.Trim();

        if (string.IsNullOrWhiteSpace(department_input))
        {
            MessageBox.Show("Department is required.");
            return;
        }

        try
        {
            PersonnelRepository.Insert_Department(department_input);
            DeptInput.Clear();
            DeptSubmit_Clicked?.Invoke(this, new EventArgs());

        } 
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            MessageBox.Show("Department already exists.");
        }
        catch (MySqlException ex)
        {
            MessageBox.Show("Database error: " + ex.Message);
        }
    }

    private void LoadDepartments()
    {
        DepartmentGrid.ItemsSource = PersonnelRepository.GetDepartments();
    }
}
