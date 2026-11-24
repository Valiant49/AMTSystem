
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AMTSystem.Data.Repositories;
using MySqlConnector;

using AMTSystem.Core.Models;

namespace AMTSystem.Wpf.Views.AssetMgmt
{
    public partial class EditAssetView : UserControl
    {
        // Stores all assets loaded into the page.
        private List<Asset> _allAssets = new();

        // Stores the currently selected asset.
        private Asset? _selectedAsset;

        // Prevents the review from refreshing while loading asset details.
        private bool _isLoading;

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

            Loaded += EditAssetView_Loaded;
        }

        // ==========================================
        // PAGE LOADING
        // ==========================================

        private void EditAssetView_Loaded(object sender, RoutedEventArgs e)
        {
            DateModifiedPicker.SelectedDate = DateTime.Today;

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

        private void AssetSearchBox_TextChanged(
    object sender,
    TextChangedEventArgs e)
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

        private void SelectAssetButton_Click(
            object sender,
            RoutedEventArgs e)
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
            ManufacturerBox.Text = asset.Manufacturer;
            ModelBox.Text = asset.Model;

            // TODO: Set the selected Category and Condition
            // after confirming their actual data types.

            _isLoading = false;

            EditForm.IsEnabled = true;

            

            RefreshReview();
        }

        // ==========================================
        // LIVE REVIEW
        // ==========================================

        private void FormTextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            RefreshReview();
        }

        private void FormSelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
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

            // Category and Condition are excluded for now.
            // Their ComboBox values need to be connected
            // to the actual Asset model first.

            

            SaveButton.IsEnabled = changes.Count > 0;
        }

        private static void Compare(
            List<FieldChange> changes,
            string field,
            string? oldValue,
            string? newValue)
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

        
        // SAVE CHANGES
       
        
        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
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

            // Apply the edited values to the selected asset.
            _selectedAsset.Name = NameBox.Text.Trim();

            _selectedAsset.SerialNumber =
                SerialNumberBox.Text.Trim();

            _selectedAsset.Manufacturer =
                ManufacturerBox.Text.Trim();

            _selectedAsset.Model =
                ModelBox.Text.Trim();

            // TODO: Save the updated asset to the database.
            // Example:
            // _assetService.Update(_selectedAsset);

            // Refresh the asset list.
            AssetGrid.Items.Refresh();

            // Reload the form to clear the Review section.
            LoadFormFromAsset(_selectedAsset);

            MessageBox.Show(
                "Asset updated in the current application data.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        
        // CANCEL CHANGES
        

        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_selectedAsset == null)
                return;

            // Restore the original saved values.
            LoadFormFromAsset(_selectedAsset);
        }

        private class ReviewAssetText
        {
        }
    }
}
 