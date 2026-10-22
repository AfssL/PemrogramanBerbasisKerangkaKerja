namespace EventKu.Helpers
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string FullName { get; set; }
        public static string Username { get; set; }
        public static string Email { get; set; }
        public static string Role { get; set; }

        public static bool IsLoggedIn => UserId > 0;

        public static void Clear()
        {
            UserId = 0;
            FullName = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            Role = string.Empty;
        }
    }
}