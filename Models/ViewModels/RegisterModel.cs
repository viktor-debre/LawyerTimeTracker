using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Not specified email")]
        [RegularExpression(@"[\w-]+@([\w-]+\.)+[\w-]+", ErrorMessage = "Wrong email format")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Not specified first name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Not specified last name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Not specified password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be {2}-{1} characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}