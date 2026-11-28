using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using AMTSystem.Core;
using AMTSystem.Core.Models;
using AMTSystem.Core.Requests;
using AMTSystem.Core.Services;
using AMTSystem.Data.Repositories;

using MySqlConnector;

namespace AMTSystem.Wpf.Views.AssetMgmt
{
    /// <summary>
    /// Interaction logic for ReturnView.xaml
    /// </summary>
    public partial class ReturnView : UserControl
    {
        private List<Personnel> _allPersonnel = new();

        private readonly AssetTransactionRepository _txRepo = new();
        private ObservableCollection<HeldItem> _heldItems = new();
        private Personnel? _selectedPersonnel;

        public List<LookupItem> Conditions { get; private set; } = new();

        private readonly AssetTransactionService _service;

        public ReturnView()
        {
            InitializeComponent();
            Loaded += ReturnView_PersonnelLoaded;
            Conditions = AssetRepository.GetAssetConditions();
            _service = new AssetTransactionService(new AssetTransactionRepository());
        }

        private void ReturnView_PersonnelLoaded(object sender, RoutedEventArgs e)
        {
            LoadPersonnel();

            PersonnelGrid.ItemsSource = _allPersonnel;
        }

        private void LoadPersonnel()
        {
            try
            {
                _allPersonnel = PersonnelRepository.GetPersonnels().ToList();
            } 
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load personnel: " + ex.Message);
            }
        }

        private void PersonnelSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = PersonnelSearchBox.Text.Trim();

            var filteredPersonnel = string.IsNullOrWhiteSpace(searchText)
                ? _allPersonnel
                : _allPersonnel.Where(a =>
                    Contains(a.FirstName, searchText) ||
                    Contains(a.LastName, searchText) ||
                    Contains(a.EmployeeNo, searchText) ||
                    Contains(a.Department, searchText))
                .ToList();

            PersonnelGrid.ItemsSource = filteredPersonnel;
        }

        private static bool Contains(string? source, string searchText)
        {
            return source?.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase) ?? false;
        }

        private void TagSearchBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private async void SelectPersonnelButton_Click(object sender, RoutedEventArgs e)
        {
            if (PersonnelGrid.SelectedItem is not Personnel picked) return;

            _selectedPersonnel = picked;
            ReviewPersonnelText.Text = picked.FullName;
            await LoadHeldItemsAsync();
        }

        private async Task LoadHeldItemsAsync()
        {
            if (_selectedPersonnel == null) return;

            try
            {
                var items = await _txRepo.GetOpenItemsByPersonnelAsync(_selectedPersonnel.Id);

                // unhook old items, hook new ones so ticks update the review strip
                foreach (var old in _heldItems) old.PropertyChanged -= HeldItem_PropertyChanged;

                _heldItems = new ObservableCollection<HeldItem>(items);
                foreach (var item in _heldItems) item.PropertyChanged += HeldItem_PropertyChanged;

                ((CollectionViewSource)Resources["HeldItemsView"]).Source = _heldItems;

                EmptyStateText.Text = "No items currently held.";
                EmptyStateText.Visibility = _heldItems.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                SelectAllCheckBox.IsChecked = false;
                UpdateReview();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load held items: " + ex.Message);
            }
        }

        private void HeldItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HeldItem.IsSelected) ||
                e.PropertyName == nameof(HeldItem.ConditionAtReturnId))
                UpdateReview();
        }

        private void UpdateReview()
        {
            int selected = _heldItems.Count(i => i.IsSelected);
            ReviewCountText.Text = $"Items to return: {selected} of {_heldItems.Count}";
            ReturnButton.IsEnabled = selected > 0;

            // TODO: set DamagedWarningText.Visibility when a selected item's condition is a "damaged" id
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool check = SelectAllCheckBox.IsChecked == true;
            foreach (var item in _heldItems) item.IsSelected = check;
        }

        private void BatchCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb || cb.Tag is not int batchId) return;

            foreach (var item in _heldItems.Where(i => i.BatchId == batchId))
                item.IsSelected = cb.IsChecked == true;
        }

        private void ConditionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private async void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            HeldItemsGrid.CommitEdit(DataGridEditingUnit.Row, true);
            var selected = _heldItems.Where(i => i.IsSelected).ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Tick at least one item to return.");
                return;
            }

            var request = new ReturnAssetsRequest
            {
                UserId = Session.CurrentUser!.Id,
                Items = selected.Select(i => new ReturnItem
                {
                    TransactionId = i.TransactionId,
                    ConditionAtReturnId = i.ConditionAtReturnId,
                    Remarks = i.ReturnRemarks
                }).ToList()
            };

            ReturnButton.IsEnabled = false;
            try
            {
                var result = await _service.ReturnAsync(request);

                if (result.Success)
                {
                    MessageBox.Show("Items returned.");
                    await LoadHeldItemsAsync();   // returned rows disappear; person stays selected
                }
                else
                {
                    MessageBox.Show(string.Join("\n", result.Errors));
                    // re-enable via UpdateReview() in finally
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message);
            }
            finally
            {
                UpdateReview();   // sets ReturnButton.IsEnabled from the current ticks
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in _heldItems) i.PropertyChanged -= HeldItem_PropertyChanged;
            _heldItems = new ObservableCollection<HeldItem>();
            ((CollectionViewSource)Resources["HeldItemsView"]).Source = _heldItems;

            _selectedPersonnel = null;
            PersonnelGrid.SelectedItem = null;
            PersonnelSearchBox.Clear();
            TagSearchBox.Clear();
            SelectAllCheckBox.IsChecked = false;

            ReviewPersonnelText.Text = "(no personnel selected)";
            EmptyStateText.Text = "Select a personnel to see their items.";
            EmptyStateText.Visibility = Visibility.Visible;

            UpdateReview();
        }
    }
}
