using System.Windows;
using System.Windows.Controls;

using AMTSystem.Data;

using MySqlConnector;

namespace AMTSystem.Wpf.Views.UserMgmt;
/// <summary>
/// Interaction logic for AllUsers.xaml
/// </summary>
public partial class AllUsers : UserControl
{
    public AllUsers()
    {
        InitializeComponent();
        Loaded += AllUsers_Loaded;
    }

    private void AllUsers_Loaded(object sender, RoutedEventArgs e){
        try
        {
            UserGrid.ItemsSource = UserRepository.GetUsers();
        } 
        catch (MySqlException ex)
        {
            MessageBox.Show("Could not load users: " + ex.Message);
        }
    }
}
