using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
using AMTSystem.Data.Repositories;

using MySqlConnector;

namespace AMTSystem.Wpf.Views.AssetMgmt
{
    public partial class EditAssetView : UserControl
    {
        private List<Asset> _allAssets = new();     // Stores all assets loaded into the page.
        private Asset? _selectedAsset;              // Stores the currently selected asset.
        private bool _isLoading;                    // Prevents the review from refreshing while loading asset details.

        // Represents a changed field in the Review section.
        private class FieldChange
        {
            public string Field { get; set; } = "";
            public string OldValue { get; set; } = "";
            public string NewValue { get; set; } = "";
        }

        public EditAssetView()
        {
            InitializeComponent();
            CategoryCombo.ItemsSource = WithPrompt(AssetRepository.GetAssetCategories(), "Select a category...");
            ConditionCombo.ItemsSource = WithPrompt(AssetRepository.GetAssetConditions(), "Select a condition...");
            Loaded += EditAssetView_Loaded;
        }

        private static List<LookupItem> WithPrompt(List<LookupItem> items, string prompt)
        {
            items.Insert(0, new LookupItem { Id = 0, Name = prompt });
            return items;
        }

        // ==========================================
        // PAGE LOADING
        // ==========================================
        private void EditAssetView_Loaded(object sender, RoutedEventArgs e)
        {
            //DateModifiedPicker.SelectedDate = DateTime.Today;

            LoadAssets();

            AssetGrid.ItemsSource = _allAssets;

            EditForm.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }

        private void LoadAssets()
        {
            try
            {
                _allAssets = AssetRepository.GetAllAssets().ToList();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Could not load assets: " + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                _allAssets = new List<Asset>();
            }
        }

        // ==========================================
        // ASSET SEARCH
        // ==========================================

        private void AssetSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = AssetSearchBox.Text.Trim();

            var filteredAssets = string.IsNullOrWhiteSpace(searchText)
                ? _allAssets
                : _allAssets
                    .Where(a =>
                        Contains(a.Tag, searchText) ||
                        Contains(a.Name, searchText) ||
                        Contains(a.SerialNumber, searchText))
                    .ToList();

            AssetGrid.ItemsSource = filteredAssets;
        }

        private static bool Contains(string? source, string searchText)
        {
            return source?.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase) ?? false;
        }

        // ==========================================
        // SELECT ASSET
        // ==========================================
        private void SelectAssetButton_Click(object sender, RoutedEventArgs e)
        {
            if (AssetGrid.SelectedItem is not Asset asset)
            {
                MessageBox.Show(
                    "Please select an asset to edit.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            _selectedAsset = asset;

            LoadFormFromAsset(asset);
        }

        // ==========================================
        // LOAD ASSET DETAILS INTO FORM
        // ==========================================
        private void LoadFormFromAsset(Asset asset)
        {
            _isLoading = true;

            TagBox.Text = asset.Tag;
            NameBox.Text = asset.Name;
            SerialNumberBox.Text = asset.SerialNumber;
            CategoryCombo.SelectedValue = asset.Category_Id;
            ConditionCombo.SelectedValue = asset.Condition_Id;
            ManufacturerBox.Text = asset.Manufacturer;
            ModelBox.Text = asset.Model;

            _isLoading = false;

            EditForm.IsEnabled = true;

            RefreshReview();
        }

        // ==========================================
        // LIVE REVIEW
        // ==========================================

        private void FormTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshReview();
        }

        private void FormSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshReview();
        }

        private void RefreshReview()
        {
            if (_isLoading || _selectedAsset == null)
                return;

            var changes = new List<FieldChange>();

            Compare(
                changes,
                "Asset Name",
                _selectedAsset.Name,
                NameBox.Text);

            Compare(
                changes,
                "Serial No.",
                _selectedAsset.SerialNumber,
                SerialNumberBox.Text);
            CompareLookup(
                changes,
                "Category",
                _selectedAsset.Category_Id,
                _selectedAsset.Category_String,
                CategoryCombo.SelectedItem as  LookupItem);
            CompareLookup(
                changes,
                "Condition",
                _selectedAsset.Condition_Id,
                _selectedAsset.Condition_String,
                ConditionCombo.SelectedItem as LookupItem);
            Compare(
                changes,
                "Manufacturer",
                _selectedAsset.Manufacturer,
                ManufacturerBox.Text);

            Compare(
                changes,
                "Model",
                _selectedAsset.Model,
                ModelBox.Text);

            SaveButton.IsEnabled = changes.Count > 0;
        }

        private static void Compare(List<FieldChange> changes, string field, string? oldValue, string? newValue)
        {
            oldValue ??= "";
            newValue = (newValue ?? "").Trim();

            if (oldValue.Trim() != newValue)
            {
                changes.Add(new FieldChange
                {
                    Field = field,
                    OldValue = oldValue,
                    NewValue = newValue
                });
            }
        }

        private static void CompareLookup(List<FieldChange> changes, string field, int? oldId, string? oldName, LookupItem? selected)
        {
            // nothing selected, or the prompt is showing: not a real change
            if (selected == null || selected.Id == 0)
                return;

            if (selected.Id != (oldId ?? 0))
            {
                changes.Add(new FieldChange
                {
                    Field = field,
                    OldValue = oldName ?? "",
                    NewValue = selected.Name
                });
            }
        }


        // SAVE CHANGES
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAsset == null)
                return;

            // Validate required fields.
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show(
                    "Asset name is required.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                NameBox.Focus();
                return;
            }

            if (CategoryCombo.SelectedItem is not LookupItem category || category.Id == 0)
            {
                MessageBox.Show(
                    "Please select a category.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                CategoryCombo.Focus();
                return;
            }

            if (ConditionCombo.SelectedItem is not LookupItem condition || condition.Id == 0)
            {
                MessageBox.Show(
                    "Please select a condition.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                ConditionCombo.Focus();
                return;
            }

            // Apply the edited values to the selected asset.
            _selectedAsset.Name = NameBox.Text.Trim();
            _selectedAsset.SerialNumber = SerialNumberBox.Text.Trim();

            _selectedAsset.Category_Id = category.Id;
            _selectedAsset.Category_String = category.Name;

            _selectedAsset.Condition_Id = condition.Id;
            _selectedAsset.Condition_String = condition.Name;

            _selectedAsset.Manufacturer = ManufacturerBox.Text.Trim();
            _selectedAsset.Model = ModelBox.Text.Trim();

            // TODO: Save the updated asset to the database.
            // Example:
            // _assetService.Update(_selectedAsset);

            AssetRepository.Update(_selectedAsset);

            // Refresh the asset list.
            AssetGrid.Items.Refresh();

            // Reload the form to clear the Review section.
            LoadFormFromAsset(_selectedAsset);

            MessageBox.Show(
                "Asset record updated.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // CANCEL CHANGES
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAsset == null)
                return;

            // Restore the original saved values.
            LoadFormFromAsset(_selectedAsset);
        }

        //private class ReviewAssetText
        //{

        //}
    }
}
