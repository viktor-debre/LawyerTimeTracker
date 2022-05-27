namespace LawyerTimeTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public Role Role { get; set; }
        public Organization Organization { get; set; }
    }
}