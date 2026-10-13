using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        private readonly StudentRepository _repository;
        public ObservableCollection<Student> Students { get; set; } = new();

        private Student? _selectedStudent = new();
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set { _selectedStudent = value; OnPropertyChanged(); }
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        // Properti Statistik Jurusan
        public int TotalStudents => Students.Count;
        public int TotalTeknikInformatika => Students.Count(x => x.Jurusan == "Teknik Informatika");
        public int TotalSistemInformasi => Students.Count(x => x.Jurusan == "Sistem Informasi");
        public int TotalTeknikKomputer => Students.Count(x => x.Jurusan == "Teknik Komputer");
        public int TotalTeknikBiomedikCount => Students.Count(x => x.Jurusan == "Teknik Biomedik");
        public int TotalTeknikElektro => Students.Count(x => x.Jurusan == "Teknik Elektro");
        public int TotalTeknologiInformasi => Students.Count(x => x.Jurusan == "Teknologi Informasi");

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand SearchCommand { get; }

        public StudentViewModel()
        {
            _repository = new StudentRepository();
            LoadData();

            SaveCommand = new RelayCommand(Save);
            DeleteCommand = new RelayCommand(Delete);
            ResetCommand = new RelayCommand(Reset);
            SearchCommand = new RelayCommand(Search);
        }

        private void LoadData()
        {
            Students.Clear();
            foreach (var s in _repository.GetAll()) Students.Add(s);
            RefreshStats();
        }

        private void Save()
        {
            if (SelectedStudent == null) return;
            if (SelectedStudent.Id == 0)
                _repository.Insert(SelectedStudent);
            else
                _repository.Update(SelectedStudent);

            LoadData();
            Reset();
        }

        private void Delete()
        {
            if (SelectedStudent == null || SelectedStudent.Id == 0) return;
            _repository.Delete(SelectedStudent.Id);
            LoadData();
            Reset();
        }

        private void Search()
        {
            var result = string.IsNullOrWhiteSpace(SearchText) ? _repository.GetAll() : _repository.Search(SearchText);
            Students.Clear();
            foreach (var s in result) Students.Add(s);
            RefreshStats();
        }

        private void Reset()
        {
            SelectedStudent = new Student();
        }

        private void RefreshStats()
        {
            OnPropertyChanged(nameof(TotalStudents));
            OnPropertyChanged(nameof(TotalTeknikInformatika));
            OnPropertyChanged(nameof(TotalSistemInformasi));
            OnPropertyChanged(nameof(TotalTeknikKomputer));
            OnPropertyChanged(nameof(TotalTeknikBiomedikCount));
            OnPropertyChanged(nameof(TotalTeknikElektro));
            OnPropertyChanged(nameof(TotalTeknologiInformasi));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
