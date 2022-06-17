using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.Models.ViewModels
{
    public class UpdateAccountModel
    {
        [Required]
        public string Email { get; set; }
        
        public string? Phonenumber { get; set; }
        public string? Skype { get; set; }
    }
}