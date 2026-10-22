using System.Windows;
using EventKu.Services;

namespace EventKu.Views
{
    public partial class LoginPage : Window
    {
        private readonly UserService _userService;

        public LoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string usernameOrEmail = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Mohon isi semua kolom!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_userService.LoginUser(usernameOrEmail, password, out string message))
            {
                MessageBox.Show(message, "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Buka MainWindow atau halaman utama aplikasi Anda di sini
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "Gagal Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnToRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            this.Close();
        }
    }
}