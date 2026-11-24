using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
using AMTSystem.Data.Repositories;

using MySqlConnector;

namespace AMTSystem.Wpf.Views.PersonnelMgmt.Departments;
/// <summary>
/// Interaction logic for AddDepartment.xaml
/// </summary>
public partial class ManageDepartments : UserControl
{
    public event EventHandler? DeptSubmit_Clicked;
    private Department? selectedDepartment;

    public ManageDepartments()
    {
        InitializeComponent();
        LoadDepartments();

    }
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DepartmentGrid.SelectedItem = null;
        selectedDepartment = null;

        DeptInput.Clear();

        DeptSubmitButton.Content = "Add";
        CancelButton.Visibility = Visibility.Collapsed;
    }
    //private void DepartmentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (DepartmentGrid.SelectedItem is Department selectedDepartment)
    //    {
    //        DeptInput.Text = selectedDepartment.DepartmentName;
    //    }
    //}
    private void DepartmentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        selectedDepartment = DepartmentGrid.SelectedItem as Department;

        if (selectedDepartment != null)
        {
            DeptInput.Text = selectedDepartment.DepartmentName;
            DeptSubmitButton.Content = "Update";
            CancelButton.Visibility = Visibility.Visible;
        }
        else
        {
            DeptInput.Clear();
            DeptSubmitButton.Content = "Add";
            CancelButton.Visibility = Visibility.Collapsed;
        }
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
            if (selectedDepartment == null)
            {
                // Add new department
                PersonnelRepository.InsertDepartment(department_input);
                MessageBox.Show("Department added successfully.");
            }
            else
            {
                // Update existing department
                // We'll add the repository method here next.
                PersonnelRepository.UpdateDepartment(
                selectedDepartment.Id,
                department_input
            );

                MessageBox.Show("Department updated successfully.");
            }

            DeptInput.Clear();
            DepartmentGrid.SelectedItem = null;
            selectedDepartment = null;

            DeptSubmitButton.Content = "Add";
            CancelButton.Visibility = Visibility.Collapsed;

            LoadDepartments();
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
