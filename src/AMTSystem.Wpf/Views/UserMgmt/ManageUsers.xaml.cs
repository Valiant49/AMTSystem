
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using AMTSystem.Core.Models;
using AMTSystem.Data.Repositories;

namespace AMTSystem.Wpf.Views.UserMgmt;

public partial class ManageUsers : UserControl
{
    private User? selectedUser;
    //private bool isLoading = false;

    public ManageUsers()
    {
        InitializeComponent();

        LoadUsers();
        LoadDropdowns();
    }

    // LOAD USERS
    private void LoadUsers()
    {
        UserGrid.ItemsSource = UserRepository.GetUsers();
    }

    // LOAD ROLE AND STATUS OPTIONS
    private void LoadDropdowns()
    {
        RoleCombo.ItemsSource = UserRepository.GetRoles();
        RoleCombo.DisplayMemberPath = "Role";
        RoleCombo.SelectedValuePath = "Id";

        StatusCombo.ItemsSource = UserRepository.GetStatuses();
        StatusCombo.DisplayMemberPath = "Status";
        StatusCombo.SelectedValuePath = "Id";
    }

    // SELECT USER FROM DATAGRID
    private void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UserGrid.SelectedItem is not User user)
            return;

        selectedUser = user;

        FirstNameBox.Text = user.FirstName;
        MiddleNameBox.Text = user.MiddleName;
        LastNameBox.Text = user.LastName;
        UsernameBox.Text = user.Username;

        RoleCombo.SelectedItem = ((List<User>)RoleCombo.ItemsSource)
            .FirstOrDefault(r => r.Role == user.Role);

        StatusCombo.SelectedValue = user.StatusId;

        EditForm.IsEnabled = true;
        SaveButton.IsEnabled = false;
    }

    // SELECT USER BUTTON
    private void SelectUserButton_Click(object sender, RoutedEventArgs e)
    {
        if (UserGrid.SelectedItem is not User)
        {
            MessageBox.Show("Please select a user first.");
            return;
        }

        UserGrid_SelectionChanged(sender, null);
    }

    // SEARCH USERS
    private void UserSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ICollectionView view = CollectionViewSource.GetDefaultView(UserGrid.ItemsSource);

        if (view == null)
            return;

        string search = UserSearchBox.Text.Trim().ToLower();

        view.Filter = item =>
        {
            if (item is not User user)
                return false;

            return user.FullName.ToLower().Contains(search)
                || user.Username.ToLower().Contains(search)
                || user.Role.ToLower().Contains(search);
        };
    }

    // DETECT TEXTBOX CHANGES
    private void FormTextChanged(object sender, TextChangedEventArgs e)
    {
        if (selectedUser == null)
            return;

        SaveButton.IsEnabled = true;
    }

    // DETECT COMBOBOX CHANGES
    private void FormSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (selectedUser == null)
            return;

        SaveButton.IsEnabled = true;
    }

    // SAVE CHANGES
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("The form is connected. Next, we need to add the database update method.");
    }

    // CANCEL EDITING
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        if (selectedUser == null)
        {
            EditForm.IsEnabled = false;
            return;
        }

        UserGrid_SelectionChanged(sender, null);
    }
}
