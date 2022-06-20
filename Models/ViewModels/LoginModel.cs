using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Not specified name")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}