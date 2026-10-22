using System.Windows;

namespace EventKu
{
    public partial class CustomMessageWindow : Window
    {
        public CustomMessageWindow(string message)
        {
            InitializeComponent();
            txtMessage.Text = message;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}