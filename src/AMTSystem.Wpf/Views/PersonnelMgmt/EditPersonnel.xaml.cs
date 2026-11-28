using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;

namespace AMTSystem.Wpf.Views.PersonnelMgmt;
/// <summary>
/// Interaction logic for EditPersonnel.xaml
/// </summary>


public partial class EditPersonnel : UserControl
{
    private readonly List<Personnel> _allPersonnel = new();  // Stores all personnel currently displayed.

    private readonly Personnel? _selectedPersonnel = new();  // Stores the currently selected personnel.

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

    //private void EditPersonnel_Loaded(object sender, RoutedEventArgs e)
    //{
        //DateModifiedPicker.SelectedDate = DateTime.Today;

        //LoadPersonnel();

        //PersonnelGrid.ItemsSource = _allPersonnel;

        //EditForm.IsEnabled = false;
        //SaveButton.IsEnabled = false;
    //}
   
    

    // ==========================================
    // PAGE LOADING
    // ==========================================

    private void EditPersonnel_Loaded(object sender, RoutedEventArgs e)
    {
        //DateModifiedPicker.SelectedDate = DateTime.Today;
        LoadPersonnel();

        PersonnelGrid.ItemsSource = _allPersonnel;

        EditForm.IsEnabled = false;
        SaveButton.IsEnabled = false;
    }

    private void LoadPersonnel()
    {
        
    }

    // ==========================================
    // PERSONNEL SEARCH
    // ==========================================

    private void PersonnelSearchBox_TextChanged(object sender,TextChangedEventArgs e)
        {
            //string searchText = PersonnelSearchBox.Text.Trim();

            //var filteredPersonnel = string.IsNullOrWhiteSpace(searchText)
            //    ? _allPersonnel
            //    : _allPersonnel
            //        .Where(p =>
            //            Contains(p.EmployeeNo, searchText) ||
            //            Contains(p.Name, searchText) ||
            //            Contains(p.Department, searchText))
            //        .ToList();

            //PersonnelGrid.ItemsSource = filteredPersonnel;
        }

    private static bool Contains(string? source, string searchText)
    {
        return source?.Contains(
            searchText,
            StringComparison.OrdinalIgnoreCase) ?? false;
    }

    // ==========================================
    // SELECT PERSONNEL
    // ==========================================

    private void SelectPersonnelButton_Click(object sender, RoutedEventArgs e)
    {
        //    if (PersonnelGrid.SelectedItem is not PersonnelRecord personnel)
        //    {
        //if (PersonnelGrid.SelectedItem is not PersonnelRecord personnel)
        //{
        //    MessageBox.Show(
        //        "Please select personnel to edit.",
        //        "No Selection",
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Warning);

        //    return;
        //}

        //_selectedPersonnel = personnel;

        //LoadFormFromPersonnel(personnel);
        //    }
    }

    // ==========================================
    // LOAD PERSONNEL DETAILS INTO FORM
    // ==========================================

    //private void LoadFormFromPersonnel(PersonnelRecord personnel)
    //{
    //_isLoading = true;

    //EmployeeNoBox.Text = personnel.EmployeeNo;
    //FirstNameBox.Text = personnel.FirstName;
    //MiddleNameBox.Text = personnel.MiddleName;
    //SurnameBox.Text = personnel.Surname;
    //PositionBox.Text = personnel.Position;
    //DepartmentBox.Text = personnel.Department;

    //EmploymentTypeCombo.SelectedIndex =
    //    EmploymentTypeCombo.Items
    //        .Cast<ComboBoxItem>()
    //        .ToList()
    //        .FindIndex(item =>
    //            item.Content?.ToString() == personnel.EmployeeType);

    //ReviewPersonnelText.Text = personnel.Name;

    //_isLoading = false;

    //EditForm.IsEnabled = true;

    //RefreshReview();
    //}

    // ==========================================
    // SAVE CHANGES
    // ==========================================

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedPersonnel == null)
            return;

        // Validate required fields.
        if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
            string.IsNullOrWhiteSpace(SurnameBox.Text))
        {
            //if (_isLoading || _selectedPersonnel == null)
            //    return;

            //var changes = new List<FieldChange>();

            //Compare(changes, "First Name",
            //    _selectedPersonnel.FirstName, FirstNameBox.Text);

            //Compare(changes, "Middle Name",
            //    _selectedPersonnel.MiddleName, MiddleNameBox.Text);

            //Compare(changes, "Surname",
            //    _selectedPersonnel.Surname, SurnameBox.Text);

            //Compare(changes, "Position",
            //    _selectedPersonnel.Position, PositionBox.Text);

            //Compare(changes, "Department",
            //    _selectedPersonnel.Department, DepartmentBox.Text);

            //string newEmploymentType =
            //    (EmploymentTypeCombo.SelectedItem as ComboBoxItem)
            //    ?.Content?.ToString() ?? "";

            //Compare(changes, "Employment Type",
            //    _selectedPersonnel.EmployeeType, newEmploymentType);

            //ReviewGrid.ItemsSource = changes;

            //ReviewCountText.Text = $"Fields changed: {changes.Count}";

            //SaveButton.IsEnabled = changes.Count > 0;
        }

        if (EmployeeTypeCombo.SelectedItem is not ComboBoxItem selectedType)
        {
            //oldValue ??= "";
            //newValue = (newValue ?? "").Trim();

            //if (oldValue.Trim() != newValue)
            //{
            //    changes.Add(new FieldChange
            //    {
            //        Field = field,
            //        OldValue = oldValue,
            //        NewValue = newValue
            //    });
            //}
        }
    }

    // ==========================================
    // SAVE CHANGES
    // ==========================================

    //private void SaveButton_Click(object sender,RoutedEventArgs e)
    //{
        //if (_selectedPersonnel == null)
        //    return;

        //if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
        //    string.IsNullOrWhiteSpace(SurnameBox.Text))
        //{
        //    MessageBox.Show(
        //        "First name and surname are required.",
        //        "Validation Error",
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Warning);

        //    return;
        //}

        //if (EmploymentTypeCombo.SelectedItem is not ComboBoxItem selectedType)
        //{
        //    MessageBox.Show(
        //        "Please select an employment type.",
        //        "Validation Error",
        //        MessageBoxButton.OK,
        //        MessageBoxImage.Warning);

        //    return;
        //}

        //// Apply edited values to the temporary personnel record.
        //_selectedPersonnel.FirstName = FirstNameBox.Text.Trim();
        //_selectedPersonnel.MiddleName = MiddleNameBox.Text.Trim();
        //_selectedPersonnel.Surname = SurnameBox.Text.Trim();
        //_selectedPersonnel.Position = PositionBox.Text.Trim();
        //_selectedPersonnel.Department = DepartmentBox.Text.Trim();

        //_selectedPersonnel.EmployeeType =
        //    selectedType.Content?.ToString() ?? "";

        //PersonnelGrid.Items.Refresh();

        //LoadFormFromPersonnel(_selectedPersonnel);

        //MessageBox.Show(
        //    "Personnel updated in the current application session.",
        //    "Success",
        //    MessageBoxButton.OK,
        //    MessageBoxImage.Information);
    //}

    // CANCEL CHANGES

        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            //if (_selectedPersonnel == null)
            //    return;

            //LoadFormFromPersonnel(_selectedPersonnel);
        }
}