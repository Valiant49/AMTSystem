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

namespace AMTSystem.Wpf.Views.PersonnelMgmt;
/// <summary>
/// Interaction logic for AllPersonnel.xaml
/// </summary>
public partial class AllPersonnel : UserControl
{
    public AllPersonnel()
    {
        InitializeComponent();
        Loaded += AllPersonnels_Loaded;
    }

    private void AllPersonnels_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            PersonnelGrid.ItemsSource = PersonnelRepository.GetPersonnels();
        }
        catch (MySqlException ex)
        {
            MessageBox.Show("Could not load personnel: " + ex.Message);
        }
    }
}
