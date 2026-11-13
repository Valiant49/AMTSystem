using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
using AMTSystem.Data;

using MySqlConnector;

namespace AMTSystem.Wpf.Views.PersonnelMgmt;
/// <summary>
/// Interaction logic for AddPersonnel.xaml
/// </summary>
public partial class AddPersonnel : UserControl
{
    private readonly Personnel _personnel = new Personnel();

    public event EventHandler? SubmitClicked;
    public AddPersonnel()
    {
        InitializeComponent();
        DepartmentBox.ItemsSource = PersonnelRepository.GetDepartments();
        DataContext = _personnel;
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        List<string> errors = _personnel.Validate();

        if (errors.Count > 0)
        {
            MessageBox.Show(string.Join("\n", errors), "Please fix the following");
            return;
        }

        try
        {
            PersonnelRepository.InsertPersonnel(_personnel);
            MessageBox.Show("Personnel record succesfully created.");
            SubmitClicked?.Invoke(this, EventArgs.Empty);
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            MessageBox.Show("An employee with this number already exists.");
        }
        catch (MySqlException ex)
        {
            MessageBox.Show("Database error: " + ex.Message);
        }
    }
}
