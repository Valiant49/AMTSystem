using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core.Models;
using AMTSystem.Data;
using AMTSystem.Wpf;
using MySqlConnector;

namespace AMTSystem.Wpf.Views.AssetMgmt;
/// <summary>
/// Interaction logic for AllAssetsView.xaml
/// </summary>
public partial class AllAssetsView : UserControl
{
    public AllAssetsView()
    {
        InitializeComponent();
        Loaded += AllAssetsView_Loaded;
    }

    private void AllAssetsView_Loaded(object sender, RoutedEventArgs e)
    {
        //throw new NotImplementedException();
        try
        {
            AssetGrid.ItemsSource = AssetRepository.GetAllAssets();
        } 
        catch (MySqlException ex)
        {
            MessageBox.Show("Could not load assets: " + ex.Message);
        }
    }
}
