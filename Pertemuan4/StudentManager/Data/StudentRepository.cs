using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using StudentManager.Models;

namespace StudentManager.Data
{
    public class StudentRepository
    {
        // Database akan otomatis dibuat menjadi file lokal bernama 'student.db'
        private readonly string connectionString = "Data Source=student.db;";

        public StudentRepository()
        {
            InitializeDatabase();
        }

        // Membuat tabel otomatis jika file database belum ada
        private void InitializeDatabase()
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string sql = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NIM TEXT NOT NULL,
                    Nama TEXT NOT NULL,
                    Jurusan TEXT NOT NULL,
                    Gender TEXT NOT NULL,
                    Email TEXT
                );
            ";
            using SqliteCommand command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();
            using SqliteConnection connection = new SqliteConnection(connectionString);
            string sql = "SELECT Id, NIM, Nama, Jurusan, Gender, Email FROM Students ORDER BY Id DESC";
            
            using SqliteCommand command = new SqliteCommand(sql, connection);
            connection.Open();
            
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student
                {
                    Id = reader.GetInt32(0),
                    NIM = reader.GetString(1),
                    Nama = reader.GetString(2),
                    Jurusan = reader.GetString(3),
                    Gender = reader.GetString(4),
                    Email = reader.IsDBNull(5) ? "" : reader.GetString(5)
                });
            }
            return students;
        }

        public void Insert(Student student)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            string sql = "INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email) VALUES (@NIM, @Nama, @Jurusan, @Gender, @Email)";
            
            using SqliteCommand command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NIM", student.NIM ?? "");
            command.Parameters.AddWithValue("@Nama", student.Nama ?? "");
            command.Parameters.AddWithValue("@Jurusan", student.Jurusan ?? "");
            command.Parameters.AddWithValue("@Gender", student.Gender ?? "");
            command.Parameters.AddWithValue("@Email", student.Email ?? "");
            
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(Student student)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            string sql = "UPDATE Students SET NIM=@NIM, Nama=@Nama, Jurusan=@Jurusan, Gender=@Gender, Email=@Email WHERE Id=@Id";
            
            using SqliteCommand command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@NIM", student.NIM ?? "");
            command.Parameters.AddWithValue("@Nama", student.Nama ?? "");
            command.Parameters.AddWithValue("@Jurusan", student.Jurusan ?? "");
            command.Parameters.AddWithValue("@Gender", student.Gender ?? "");
            command.Parameters.AddWithValue("@Email", student.Email ?? "");
            
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            using SqliteCommand command = new SqliteCommand("DELETE FROM Students WHERE Id=@Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<Student> Search(string keyword)
        {
            var students = new List<Student>();
            using SqliteConnection connection = new SqliteConnection(connectionString);
            string sql = "SELECT Id, NIM, Nama, Jurusan, Gender, Email FROM Students WHERE NIM LIKE @Keyword OR Nama LIKE @Keyword OR Jurusan LIKE @Keyword ORDER BY Id DESC";
            
            using SqliteCommand command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
            
            connection.Open();
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student
                {
                    Id = reader.GetInt32(0),
                    NIM = reader.GetString(1),
                    Nama = reader.GetString(2),
                    Jurusan = reader.GetString(3),
                    Gender = reader.GetString(4),
                    Email = reader.IsDBNull(5) ? "" : reader.GetString(5)
                });
            }
            return students;
        }
    }
}