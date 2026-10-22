using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventKu
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();
        }

        private void NavigateDashboard(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("DashboardPage.xaml", System.UriKind.Relative);
            SetActiveButton(BtnDashboard);
        }

        private void NavigateCategory(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("CategoryPage.xaml", System.UriKind.Relative);
            SetActiveButton(BtnCategory);
        }

        private void NavigateReviews(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("ReviewsPage.xaml", System.UriKind.Relative);
            SetActiveButton(BtnReviews);
        }

        private void NavigateArchive(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("ArchivePage.xaml", System.UriKind.Relative);
            SetActiveButton(BtnArchive);
        }

        private void NavigateSettings(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new System.Uri("SettingsPage.xaml", System.UriKind.Relative);
            SetActiveButton(BtnSettings);
        }

        private void SetActiveButton(Button activeBtn)
        {
            if (BtnDashboard.Parent is Border b1) b1.Background = Brushes.Transparent;
            if (BtnCategory.Parent is Border b2) b2.Background = Brushes.Transparent;
            if (BtnReviews.Parent is Border b3) b3.Background = Brushes.Transparent;
            if (BtnArchive.Parent is Border b4) b4.Background = Brushes.Transparent;
            if (BtnSettings.Parent is Border b5) b5.Background = Brushes.Transparent;

            Brush grayBrush = (Brush)(new BrushConverter().ConvertFrom("#94A3B8") ?? Brushes.Gray);
            BtnDashboard.Foreground = grayBrush;
            BtnCategory.Foreground = grayBrush;
            BtnReviews.Foreground = grayBrush;
            BtnArchive.Foreground = grayBrush;
            BtnSettings.Foreground = grayBrush;

            if (activeBtn.Parent is Border activeBorder)
            {
                activeBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#00529B") ?? Brushes.Blue);
            }
            activeBtn.Foreground = Brushes.White;
        }
    }
}