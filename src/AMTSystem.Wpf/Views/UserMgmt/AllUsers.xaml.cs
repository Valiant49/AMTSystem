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
