namespace Ecom_WebAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Note: In production, consider storing a password hash instead of the raw password.
        public string FullName { get; set; }
    }
}
