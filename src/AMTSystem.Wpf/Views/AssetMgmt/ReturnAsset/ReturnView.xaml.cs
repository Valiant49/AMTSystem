using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using AMTSystem.Core.Models;
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

        public List<AssetCondition> Conditions { get; private set; } = new();

        public ReturnView()
        {
            InitializeComponent();
            Loaded += ReturnView_Loaded;
        }

        private void ReturnView_Loaded(object sender, RoutedEventArgs e)
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

            var filteredPersonnel = string.IsNullOrWhiteSpace(searchText);
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

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConditionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
