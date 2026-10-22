using System.Linq;
using System.Windows.Controls;

namespace EventKu
{
    public partial class ArchivePage : Page
    {
        public ArchivePage()
        {
            InitializeComponent();
            LoadArchive();
        }

        private void LoadArchive()
        {
            var events = DatabaseHelper.GetAllEvents();
            var completedEvents = events.Where(e => e.Status.Equals("Completed", System.StringComparison.OrdinalIgnoreCase)).ToList();
            dgArchive.ItemsSource = completedEvents;
        }
    }
}