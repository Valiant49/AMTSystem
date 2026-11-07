using System.Windows;
using System.Windows.Controls;

using MySqlConnector;

using AMTSystem.Core.Models;
using AMTSystem.Core.Helpers;
using AMTSystem.Data;

namespace AMTSystem.Wpf.Views.AssetMgmt;

/// <summary>
/// Interaction logic for RegisterView.xaml
/// </summary>
public partial class RegisterView : UserControl
{
    public event EventHandler? SubmitClicked;

    public RegisterView(AssetDraft draft)
    {
        InitializeComponent();
        CategoryBox.ItemsSource = WithPrompt(AssetRepository.GetCategories(), "Select a category...");
        StatusBox.ItemsSource = WithPrompt(AssetRepository.GetStatuses(), "Please select a status...");
        ConditionBox.ItemsSource = WithPrompt(AssetRepository.GetConditions(), "Select a condition...");
        ManufacturerBox.ItemsSource = AssetRepository.GetManufacturers();
        
        DataContext = draft;
        LoadModels();
    }

    //private void Next_Click(object sender, RoutedEventArgs e)
    //{
    //    if (NextClicked != null)
    //    {
    //        NextClicked(this, EventArgs.Empty);
    //    }
    //}




    private void Submit_Clicked(object sender, RoutedEventArgs e)
    {
        AssetDraft draft = (AssetDraft)DataContext;
        draft.Normalize();
        draft.Tag = ReferenceNo.GenerateAssetRef(8);

        List<string> errors = draft.Validate();

        if (errors.Count > 0)
        {
            MessageBox.Show(string.Join("\n", errors), "Please fix the following");
            return;
        }

        try
        {
            AssetRepository.Insert(draft);
            SubmitClicked?.Invoke(this, EventArgs.Empty);
        } 
        //catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        //{
        //    MessageBox.Show("An asset with that serial number already exists.");
        //} 
        catch (MySqlException ex)
        {
            MessageBox.Show("Database error: " + ex.Message);
        }
        finally
        {

        }
        
    }

    private static List<LookupItem> WithPrompt(List<LookupItem> items, string prompt)
    {
        items.Insert(0, new LookupItem { Id = 0, Name = prompt });
        return items;
    }

    private void ModelBox_DropdownOpened(object? sender, EventArgs e)
    {
        LoadModels();
    }

    private void LoadModels()
    {
        string currentModel = ModelBox.Text;
        ModelBox.ItemsSource = AssetRepository.GetModels(ManufacturerBox.Text.Trim());
        ModelBox.Text = currentModel;
    }
}
