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

    namespace AMTSystem.Wpf.Views.AssetMgmt;

    /// <summary>
    /// Interaction logic for IssueView.xaml
    /// </summary>
    public partial class IssueView : UserControl
    {
        private readonly AssetIssuanceService _service;
        //private readonly Session _session;

        //preps the list for the grids to store entity records
        private ObservableCollection<Personnel> _personnelList = new();
        private ICollectionView _personnelView;
        private ObservableCollection<Asset> _availableAssets = new();
        private ObservableCollection<Asset> _assetsToIssue = new();
        private Personnel? _selectedPersonnel;

        public IssueView()
        {
            InitializeComponent();
            Loaded += AllAssetsIssueView_Loaded;
            Loaded += AllPersonnels_Loaded;
            IssuingOfficerText.Text = Session.CurrentUser?.FullName;
        
            _service = new AssetIssuanceService(new AssetTransactionRepository());

            var txRepo = new AssetTransactionRepository();
            var service = new AssetIssuanceService(txRepo);
            //var view = new IssueView(service);
        }

        private void AllAssetsIssueView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _availableAssets = new ObservableCollection<Asset>(AssetRepository.GetAvailableAssets());
                AvailableAssetGrid.ItemsSource = _availableAssets;
                ReviewGrid.ItemsSource = _assetsToIssue;

                var view = CollectionViewSource.GetDefaultView(_availableAssets);
                view.Filter = o => o is Asset a &&
                    (string.IsNullOrWhiteSpace(AssetSearchBox.Text) ||
                     a.Tag.Contains(AssetSearchBox.Text, StringComparison.OrdinalIgnoreCase));

                AssetSearchBox.TextChanged += (s, e) => view.Refresh();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load assets: " + ex.Message);
            }
        }

        private void AllPersonnels_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _personnelList = new ObservableCollection<Personnel>(PersonnelRepository.GetPersonnels());
                PersonnelGrid.ItemsSource = _personnelList;

                _personnelView = CollectionViewSource.GetDefaultView(_personnelList);
                _personnelView.Filter = FilterPersonnel;

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load personnel: " + ex.Message);
            }
        }

        private bool FilterPersonnel(object item)
        {
            if (item is not Personnel p) return false;

            var text = PersonnelSearchBox.Text;
            if (string.IsNullOrWhiteSpace(text)) return true;

            return p.FullName.Contains(text, StringComparison.OrdinalIgnoreCase)
                || p.EmployeeNo.Contains(text, StringComparison.OrdinalIgnoreCase)
                || (p.Department?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private void SelectPersonnelButton_Click(object sender, RoutedEventArgs e)
        {
            if (PersonnelGrid.SelectedItem is not Personnel picked) return;

            if (_selectedPersonnel != null)
                _personnelList.Add(_selectedPersonnel);

            _personnelList.Remove(picked);
            _selectedPersonnel = picked;
            ReviewPersonnelText.Text = picked.FullName;
        }


        private void AddAssetButton_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableAssetGrid.SelectedItem is not Asset assetPicked) return;

            _availableAssets.Remove(assetPicked);
            _assetsToIssue.Add(assetPicked);

            ReviewCountText.Text = $"Assets to issue: {_assetsToIssue.Count}";
        }
    

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // return assets to _availableAssets grid
            foreach (var asset in _assetsToIssue)
                _availableAssets.Add(asset);
            _assetsToIssue.Clear();

            // returns chosen personnel back to _personnelList
            if (_selectedPersonnel != null)
            {
                _personnelList.Add(_selectedPersonnel);
                _selectedPersonnel = null;
            }



            AvailableAssetGrid.SelectedItems.Clear();
            PersonnelGrid.SelectedItem = null;

            PersonnelSearchBox.Clear();
            AssetSearchBox.Clear();
            RemarksBox.Clear();
            DateIssuedPicker.SelectedDate = DateTime.Today;

            ReviewPersonnelText.Text = "(no personnel selected)";
            ReviewCountText.Text = "Assets to issue: 0";
        }

        private async void IssueButton_Click(object sender, RoutedEventArgs e)
        {
            // checking against null selections
            if (_selectedPersonnel == null)
            {
                MessageBox.Show("Select a personnel.");
                return;
            }
            if (_assetsToIssue.Count == 0)
            {
                MessageBox.Show("Add at least one asset.");
                return;
            }

            // converts the selected data into IDs and plain values only
            var request = new IssueAssetsRequest
            {
                UserId = Session.CurrentUser!.Id,
                PersonnelId = _selectedPersonnel.Id,
                Type = TransactionType.Issued,
                Remarks = string.IsNullOrWhiteSpace(RemarksBox.Text) ? null : RemarksBox.Text,
                Items = _assetsToIssue.Select(a => new IssueItem
                {
                    AssetId = a.Id,
                    ConditionId = a.Condition_Id   // whatever your Asset property is called
                }).ToList()
            };

            // calls the assset issuance service
            IssueButton.IsEnabled = false;

            try
            {
                var result = await _service.IssueAsync(request);

                if (result.Success)
                {
                    MessageBox.Show("Assets issued.");
                    ResetAfterIssue();
                }
                else
                {
                    MessageBox.Show(string.Join("\n", result.Errors));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message);
            }
            finally
            {
                IssueButton.IsEnabled = true;
            }

        }

        private void ResetAfterIssue()
        {
            _assetsToIssue.Clear();

            if (_selectedPersonnel != null)
            {
                _personnelList.Add(_selectedPersonnel);
                _selectedPersonnel = null;
            }

            PersonnelSearchBox.Clear();
            AssetSearchBox.Clear();
            RemarksBox.Clear();
            ReviewPersonnelText.Text = "(no personnel selected)";
            ReviewCountText.Text = "Assets to issue: 0";
        }


        // automates barcode scanning from reading tag
        // to transferring the read asset from one grid to another
        // if the read Key of the barcode scanner is 'Enter'
        private void AssetSearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var matches = AvailableAssetGrid.Items.Cast<Asset>().ToList();
            if (matches.Count == 1)
            {
                _availableAssets.Remove(matches[0]);
                _assetsToIssue.Add(matches[0]);
                AssetSearchBox.Clear();
            }
        }


        private void PersonnelSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _personnelView?.Refresh();
        }
    }