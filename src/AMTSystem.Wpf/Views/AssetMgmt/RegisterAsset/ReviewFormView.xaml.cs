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

using AMTSystem.Core.Models;

namespace AMTSystem.Wpf.Views.AssetMgmt;
/// <summary>
/// Interaction logic for ReviewFormView.xaml
/// </summary>
public partial class ReviewFormView : UserControl
{
    public event EventHandler BackClicked;
    public event EventHandler SubmitClicked;

    public ReviewFormView(AssetDraft draft)
    {
        InitializeComponent();
        DataContext = draft;
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        BackClicked?.Invoke(this, EventArgs.Empty);
    }

    private void Submit_Click(object sender, RoutedEventArgs e)
    {
        SubmitClicked?.Invoke(this, EventArgs.Empty);
    }
}
