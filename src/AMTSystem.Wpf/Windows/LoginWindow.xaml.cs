using System.Windows;

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
        bool isUsernameFilled = !string.IsNullOrWhiteSpace(txbUsername.Text);
        bool isPasswordFilled = !string.IsNullOrWhiteSpace(txbPassword.Password);

        if (isUsernameFilled && isPasswordFilled)
        {
            btnLogin.IsEnabled = true;
        }
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        var main = new MainWindow();
        Application.Current.MainWindow = main;
        main.Show();
        Close();
    }
}