using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;

namespace EventKu
{
    public class DatabaseHelper
    {
        // Menggunakan koneksi LocalDB dengan direktori mdf lokal agar tidak terkena error permission login user Windows
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EventKuDB.mdf");
        private static string connectionString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;";
        private static string masterConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

        public static void InitializeDatabase()
        {
            // Pastikan database master hidup, lalu buat database jika belum ada
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                string checkDbQuery = "SELECT database_id FROM sys.databases WHERE Name = 'EventKuDB'";
                bool dbExists = false;
                using (var checkCmd = new SqlCommand(checkDbQuery, connection))
                {
                    var result = checkCmd.ExecuteScalar();
                    if (result != null) dbExists = true;
                }

                if (!dbExists)
                {
                    string createDbQuery = $"CREATE DATABASE EventKuDB ON PRIMARY (NAME = EventKuDB_Data, FILENAME = '{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EventKuDB.mdf")}')";
                    using (var createCmd = new SqlCommand(createDbQuery, connection))
                    {
                        createCmd.ExecuteNonQuery();
                    }
                }
            }

            // Buat tabel Events jika belum ada
            using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=EventKuDB;Trusted_Connection=True;"))
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' and xtype='U')
                    CREATE TABLE Events (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Title NVARCHAR(150) NOT NULL,
                        Category NVARCHAR(50) NOT NULL,
                        EventDate NVARCHAR(50) NOT NULL,
                        Location NVARCHAR(150) NOT NULL,
                        Status NVARCHAR(50) NOT NULL
                    )";
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<EventModel> GetAllEvents()
        {
            var events = new List<EventModel>();
            using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=EventKuDB;Trusted_Connection=True;"))
            {
                connection.Open();
                string query = "SELECT Id, Title, Category, EventDate, Location, Status FROM Events";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new EventModel
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Category = reader.GetString(2),
                                EventDate = reader.GetString(3),
                                Location = reader.GetString(4),
                                Status = reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return events;
        }

        public static void InsertEvent(string title, string category, string date, string location, string status)
        {
            using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=EventKuDB;Trusted_Connection=True;"))
            {
                connection.Open();
                string query = "INSERT INTO Events (Title, Category, EventDate, Location, Status) VALUES (@Title, @Category, @EventDate, @Location, @Status)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@EventDate", date);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Status", status);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteEvent(int id)
        {
            using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=EventKuDB;Trusted_Connection=True;"))
            {
                connection.Open();
                
                // Hapus data berdasarkan ID
                string query = "DELETE FROM Events WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                // Cek apakah tabel sudah kosong. Jika kosong, reset total ID kembali ke 0 (sehingga insert berikutnya jadi ID 1)
                string checkCountQuery = "SELECT COUNT(*) FROM Events";
                int remainingCount = 0;
                using (var countCmd = new SqlCommand(checkCountQuery, connection))
                {
                    remainingCount = (int)countCmd.ExecuteScalar();
                }

                if (remainingCount == 0)
                {
                    string resetQuery = "DBCC CHECKIDENT ('Events', RESEED, 0)";
                    using (var resetCmd = new SqlCommand(resetQuery, connection))
                    {
                        resetCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Jika masih ada sisa data, atur ulang seed ke nilai ID maksimum yang tersisa
                    string reseedQuery = "DECLARE @MaxID INT; SELECT @MaxID = MAX(Id) FROM Events; DBCC CHECKIDENT ('Events', RESEED, @MaxID);";
                    using (var reseedCmd = new SqlCommand(reseedQuery, connection))
                    {
                        reseedCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public static void UpdateEvent(int id, string title, string category, string date, string location, string status)
        {
            using (var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=EventKuDB;Trusted_Connection=True;"))
            {
                connection.Open();
                string query = "UPDATE Events SET Title = @Title, Category = @Category, EventDate = @EventDate, Location = @Location, Status = @Status WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@EventDate", date);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Status", status);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}