using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;

namespace EventKu
{
    public partial class CategoryPage : Page
    {
        public CategoryPage()
        {
            InitializeComponent();
        }

        private void Category_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string categoryName)
            {
                var allEvents = DatabaseHelper.GetAllEvents();
                var filtered = allEvents.Where(x => x.Category.Equals(categoryName, System.StringComparison.OrdinalIgnoreCase)).ToList();

                string message = filtered.Count > 0 
                    ? $"Ditemukan {filtered.Count} event untuk kategori '{categoryName}':\n\n- " + string.Join("\n- ", filtered.Select(x => $"{x.Title} ({x.EventDate})"))
                    : $"Belum ada event terdaftar untuk kategori '{categoryName}'.";

                new CustomMessageWindow(message).ShowDialog();
            }
        }
    }
}