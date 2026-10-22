using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EventKu
{
    public partial class DashboardPage : Page
    {
        private int selectedEventId = -1;

        public DashboardPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var events = DatabaseHelper.GetAllEvents();
                dgEvents.ItemsSource = events;
                
                txtTotalEvent.Text = events.Count.ToString();
                txtUpcomingEvent.Text = events.Count(e => e.Status.Equals("Upcoming", StringComparison.OrdinalIgnoreCase)).ToString();
                txtCompletedEvent.Text = events.Count(e => e.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)).ToString();
            }
            catch (Exception ex)
            {
                new CustomMessageWindow("Gagal memuat data: " + ex.Message).ShowDialog();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                string title = txtTitle.Text;
                string category = cbCategory.SelectedItem is ComboBoxItem itemCat && itemCat.Content != null ? itemCat.Content.ToString() ?? "" : "";
                string date = txtDate.Text;
                string location = txtLocation.Text;
                string status = cbStatus.SelectedItem is ComboBoxItem itemStat && itemStat.Content != null ? itemStat.Content.ToString() ?? "" : "";

                DatabaseHelper.InsertEvent(title, category, date, location, status);
                LoadData();
                ClearForm();
                new CustomMessageWindow("Event berhasil ditambahkan!").ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageWindow("Gagal menyimpan data: " + ex.Message).ShowDialog();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEventId == -1)
            {
                new CustomMessageWindow("Pilih baris data pada tabel yang ingin di-update terlebih dahulu!").ShowDialog();
                return;
            }

            if (!ValidateForm()) return;

            try
            {
                string title = txtTitle.Text;
                string category = cbCategory.SelectedItem is ComboBoxItem itemCat && itemCat.Content != null ? itemCat.Content.ToString() ?? "" : "";
                string date = txtDate.Text;
                string location = txtLocation.Text;
                string status = cbStatus.SelectedItem is ComboBoxItem itemStat && itemStat.Content != null ? itemStat.Content.ToString() ?? "" : "";

                DatabaseHelper.UpdateEvent(selectedEventId, title, category, date, location, status);
                LoadData();
                ClearForm();
                new CustomMessageWindow("Event berhasil diperbarui!").ShowDialog();
            }
            catch (Exception ex)
            {
                new CustomMessageWindow("Gagal memperbarui data: " + ex.Message).ShowDialog();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgEvents.SelectedItem is EventModel selected)
            {
                try
                {
                    DatabaseHelper.DeleteEvent(selected.Id);
                    LoadData();
                    ClearForm();
                    new CustomMessageWindow("Event berhasil dihapus! ID telah diurutkan ulang.").ShowDialog();
                }
                catch (Exception ex)
                {
                    new CustomMessageWindow("Gagal menghapus data: " + ex.Message).ShowDialog();
                }
            }
            else
            {
                new CustomMessageWindow("Pilih baris pada tabel yang ingin dihapus!").ShowDialog();
            }
        }

        private void DgEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEvents.SelectedItem is EventModel selected)
            {
                selectedEventId = selected.Id;
                txtTitle.Text = selected.Title;
                txtDate.Text = selected.EventDate;
                txtLocation.Text = selected.Location;

                foreach (ComboBoxItem item in cbCategory.Items)
                {
                    if (item.Content?.ToString() == selected.Category)
                    {
                        cbCategory.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cbStatus.Items)
                {
                    if (item.Content?.ToString() == selected.Status)
                    {
                        cbStatus.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || 
                cbCategory.SelectedItem == null || 
                string.IsNullOrWhiteSpace(txtDate.Text) || 
                string.IsNullOrWhiteSpace(txtLocation.Text) || 
                cbStatus.SelectedItem == null)
            {
                new CustomMessageWindow("Mohon isi semua field terlebih dahulu!").ShowDialog();
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            selectedEventId = -1;
            txtTitle.Clear();
            txtDate.Clear();
            txtLocation.Clear();
            cbCategory.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;
            dgEvents.UnselectAll();
        }
    }
}