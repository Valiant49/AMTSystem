using System.Windows;
using System.Windows.Controls;

using AMTSystem.Core;
using AMTSystem.Core.Models;
using AMTSystem.Data;

using MySqlConnector;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AMTSystem.Wpf.Views.UserMgmt
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : UserControl
    {
        private readonly User _user = new User();
        public event EventHandler? SubmitClick;

        public AddUser()
        {
            InitializeComponent();
            RoleBox.ItemsSource = UserRepository.GetRoles();
            AccountStatusBox.ItemsSource = UserRepository.GetStatuses();
            DataContext = _user;
        }

        private static List<LookupItem> WithPrompt(List<LookupItem> items, string prompt)
        {
            items.Insert(0, new LookupItem { Id = 0, Name = prompt });
            return items;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = _user.Validate();

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
                errors.Add("Passwords do not match");

            if (PasswordBox.Password.Length < 8 || ConfirmPasswordBox.Password.Length < 8)
                errors.Add("Password must be at least 8 characters");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Please fix the following");
                return;
            }

            _user.PasswordHash = PasswordHasher.Hash(PasswordBox.Password);

            for (int attempt = 0; attempt < 5; attempt++)
            {
                _user.Username = User.GenerateUsername(_user.FirstName, _user.LastName);

                try
                {
                    UserRepository.AddUser(_user);
                    MessageBox.Show("User succesfully created");
                    SubmitClick?.Invoke(this, EventArgs.Empty);
                    return;
                }
                catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    //MessageBox.Show("Someone with this username already exists.");
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
            MessageBox.Show("Could not generate a unique username. Please try again.");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FirstNameBox.Clear();
            MiddleNameBox.Clear();
            LastNameBox.Clear();
            UsernameBox.Clear();
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();
            RoleBox.Text = "Please select a role...";
            AccountStatusBox.Text = "Please select a status...";
            CreatedAtBox.Clear();
            UpdatedAtBox.Clear();
        }

        private void NameBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (UsernameBox == null) return;
            UsernameBox.Text = User.GenerateUsernameBase(FirstNameBox.Text, LastNameBox.Text);
        }

    }
}
