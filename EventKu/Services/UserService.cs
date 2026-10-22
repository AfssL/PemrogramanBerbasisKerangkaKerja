using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace EventKu.Services
{
    public class UserService
    {
        private readonly string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=EventKuDB;Trusted_Connection=True;";

        public UserService()
        {
            // Otomatis buat tabel Users jika belum ada saat aplikasi berjalan
            EnsureTableCreated();
        }

        private void EnsureTableCreated()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
                        CREATE TABLE Users (
                            UserId INT IDENTITY(1,1) PRIMARY KEY,
                            FullName NVARCHAR(100) NOT NULL,
                            Username NVARCHAR(50) NOT NULL UNIQUE,
                            Email NVARCHAR(100) NOT NULL UNIQUE,
                            PasswordHash NVARCHAR(255) NOT NULL,
                            Role NVARCHAR(20) DEFAULT 'User',
                            CreatedAt DATETIME DEFAULT GETDATE()
                        );";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Tangani error koneksi jika diperlukan
            }
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool RegisterUser(string fullName, string username, string email, string password, out string message)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (FullName, Username, Email, PasswordHash, Role) VALUES (@FullName, @Username, @Email, @PasswordHash, 'User')";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));

                        command.ExecuteNonQuery();
                    }
                }
                message = "Registrasi berhasil!";
                return true;
            }
            catch (Exception ex)
            {
                message = "Gagal registrasi: " + ex.Message;
                return false;
            }
        }

        public bool LoginUser(string usernameOrEmail, string password, out string message)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT UserId, FullName, Username, Email, PasswordHash, Role FROM Users WHERE Username = @Input OR Email = @Input";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Input", usernameOrEmail);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["PasswordHash"].ToString();
                                string inputHash = HashPassword(password);

                                if (storedHash == inputHash)
                                {
                                    Helpers.UserSession.UserId = Convert.ToInt32(reader["UserId"]);
                                    Helpers.UserSession.FullName = reader["FullName"].ToString();
                                    Helpers.UserSession.Username = reader["Username"].ToString();
                                    Helpers.UserSession.Email = reader["Email"].ToString();
                                    Helpers.UserSession.Role = reader["Role"].ToString();

                                    message = "Login berhasil!";
                                    return true;
                                }
                            }
                        }
                    }
                }
                message = "Username/Email atau Password salah!";
                return false;
            }
            catch (Exception ex)
            {
                message = "Terjadi kesalahan: " + ex.Message;
                return false;
            }
        }
    }
}