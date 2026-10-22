using System.Windows;
using EventKu.Services;

namespace EventKu.Views
{
    public partial class RegisterPage : Window
    {
        private readonly UserService _userService;

        public RegisterPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_userService.RegisterUser(fullName, username, email, password, out string message))
            {
                MessageBox.Show(message, "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginPage loginPage = new LoginPage();
                loginPage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Close();
        }
    }
}