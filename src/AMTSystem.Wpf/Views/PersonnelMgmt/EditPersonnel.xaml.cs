using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMTSystem.Wpf.Views.PersonnelMgmt;
/// <summary>
/// Interaction logic for EditPersonnel.xaml
/// </summary>


    public partial class EditPersonnel : UserControl
{
    // Stores all personnel currently displayed.
    private List<PersonnelRecord> _allPersonnel = new();

    // Stores the currently selected personnel.
    private PersonnelRecord? _selectedPersonnel;

    public EditPersonnel()
    {
        InitializeComponent();

        Loaded += EditPersonnel_Loaded;
    }
    private void FormTextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void FormSelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }




    // ==========================================
    // TEMPORARY PERSONNEL CLASS
    // ==========================================

    private class PersonnelRecord
    {
        public string EmployeeNo { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Position { get; set; } = "";
        public string Department { get; set; } = "";
        public string EmployeeType { get; set; } = "";

        public string Name =>
            $"{FirstName} {MiddleName} {Surname}"
            .Replace("  ", " ").Trim();
    }

    // ==========================================
    // PAGE LOADING
    // ==========================================

    private void EditPersonnel_Loaded(
        object sender,
        RoutedEventArgs e)
    {
       

        LoadPersonnel();

        PersonnelGrid.ItemsSource = _allPersonnel;

        EditForm.IsEnabled = false;
        SaveButton.IsEnabled = false;
    }

    private void LoadPersonnel()
    {
        // Temporary sample data.
        // Database connection will be added later.

        _allPersonnel = new List<PersonnelRecord>
            {
                new PersonnelRecord
                {
                    EmployeeNo = "EMP-001",
                    FirstName = "Juan",
                    MiddleName = "Santos",
                    Surname = "Dela Cruz",
                    Position = "IT Officer",
                    Department = "IT Department",
                    EmployeeType = "Full-Time"
                },

                new PersonnelRecord
                {
                    EmployeeNo = "EMP-002",
                    FirstName = "Maria",
                    MiddleName = "Reyes",
                    Surname = "Santos",
                    Position = "Administrative Assistant",
                    Department = "Administration",
                    EmployeeType = "Part-Time"
                },

                new PersonnelRecord
                {
                    EmployeeNo = "EMP-003",
                    FirstName = "Pedro",
                    MiddleName = "",
                    Surname = "Garcia",
                    Position = "Technician",
                    Department = "Maintenance",
                    EmployeeType = "Full-Time"
                }
            };
    }

    // ==========================================
    // PERSONNEL SEARCH
    // ==========================================

    private void PersonnelSearchBox_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        string searchText = PersonnelSearchBox.Text.Trim();

        var filteredPersonnel = string.IsNullOrWhiteSpace(searchText)
            ? _allPersonnel
            : _allPersonnel
                .Where(p =>
                    Contains(p.EmployeeNo, searchText) ||
                    Contains(p.Name, searchText) ||
                    Contains(p.Department, searchText))
                .ToList();

        PersonnelGrid.ItemsSource = filteredPersonnel;
    }

    private static bool Contains(
        string? source,
        string searchText)
    {
        return source?.Contains(
            searchText,
            StringComparison.OrdinalIgnoreCase) ?? false;
    }

    // ==========================================
    // SELECT PERSONNEL
    // ==========================================

    private void SelectPersonnelButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (PersonnelGrid.SelectedItem is not PersonnelRecord personnel)
        {
            MessageBox.Show(
                "Please select personnel to edit.",
                "No Selection",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        _selectedPersonnel = personnel;

        LoadFormFromPersonnel(personnel);
    }

    // ==========================================
    // LOAD PERSONNEL DETAILS
    // ==========================================

    private void LoadFormFromPersonnel(
        PersonnelRecord personnel)
    {
        EmployeeNoBox.Text = personnel.EmployeeNo;
        FirstNameBox.Text = personnel.FirstName;
        MiddleNameBox.Text = personnel.MiddleName;
        SurnameBox.Text = personnel.Surname;
        PositionBox.Text = personnel.Position;
        DepartmentBox.Text = personnel.Department;

        EmployeeTypeCombo.SelectedIndex =
            EmployeeTypeCombo.Items
                .Cast<ComboBoxItem>()
                .ToList()
                .FindIndex(item =>
                    item.Content?.ToString() == personnel.EmployeeType);

        EditForm.IsEnabled = true;
        SaveButton.IsEnabled = false;
    }

    // ==========================================
    // SAVE CHANGES
    // ==========================================

    private void SaveButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (_selectedPersonnel == null)
            return;

        // Validate required fields.
        if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
            string.IsNullOrWhiteSpace(SurnameBox.Text))
        {
            MessageBox.Show(
                "First name and surname are required.",
                "Validation Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (EmployeeTypeCombo.SelectedItem is not ComboBoxItem selectedType)
        {
            MessageBox.Show(
                "Please select an employee type.",
                "Validation Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        // Apply edited values to the selected personnel.
        _selectedPersonnel.FirstName = FirstNameBox.Text.Trim();
        _selectedPersonnel.MiddleName = MiddleNameBox.Text.Trim();
        _selectedPersonnel.Surname = SurnameBox.Text.Trim();
        _selectedPersonnel.Position = PositionBox.Text.Trim();
        _selectedPersonnel.Department = DepartmentBox.Text.Trim();

        _selectedPersonnel.EmployeeType =
            selectedType.Content?.ToString() ?? "";

        // Refresh the personnel list.
        PersonnelGrid.Items.Refresh();

        // Reload the updated personnel details.
        LoadFormFromPersonnel(_selectedPersonnel);

        MessageBox.Show(
            "Personnel updated successfully!",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    // CANCEL CHANGES

    private void CancelButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (_selectedPersonnel == null)
            return;

        // Restore the original values.
        LoadFormFromPersonnel(_selectedPersonnel);
    }
}
