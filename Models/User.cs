namespace LawyerTimeTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public Role Role { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public byte[]? Image { get; set; }
        public string? Phonenumber { get; set; }
        public string? Skype { get; set; }
    }
}