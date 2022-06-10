using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.ViewModels
{
    public class RegisterModel
    {
        //validation need to move to the client-side
        [Required(ErrorMessage = "Not specified email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Not specified first name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Not specified last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wrong password")]
        public string ConfirmPassword { get; set; }
    }
}