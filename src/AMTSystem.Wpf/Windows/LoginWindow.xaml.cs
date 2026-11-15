using System.Windows;

using AMTSystem.Core;

namespace AMTSystem.Wpf.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        btnLogin.IsEnabled = false;
    }

    private void InputField_Changed(object sender, RoutedEventArgs e)
    {
        bool isUsernameFilled = !string.IsNullOrWhiteSpace(UsernameBox.Text);
        bool isPasswordFilled = !string.IsNullOrWhiteSpace(PasswordBox.Password);

        if (isUsernameFilled && isPasswordFilled)
        {
            btnLogin.IsEnabled = true;
        }
    }

    private async void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        btnLogin.IsEnabled=false;
        string username = UsernameBox.Text;
        string password = PasswordBox.Password;
        PasswordBox.Clear();
        var user = await Task.Run(() => AuthService.Login(username, password));

        if (user == null)
        {
            ErrorText.Text = "Invalid username or password.";
            btnLogin.IsEnabled = true;
            return;
        }

        Session.Start(user);
        var main = new MainWindow();
        Application.Current.MainWindow = main;
        main.Show();
        Close();
    }
}