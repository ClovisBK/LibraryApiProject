namespace LibrarySystemApi.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
