using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.Models.ViewModels
{
    public class UpdateAccountModel
    {
        [Required] public string Email { get; set; }
        public bool IsImageToDelete { get; set; }
        public byte[]? Image { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Skype { get; set; }
    }
}