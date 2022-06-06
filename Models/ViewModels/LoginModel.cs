using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.ViewModels
{
    public class LoginModel
    {
        //validation need to move to the client-side
        [Required(ErrorMessage = "Not specified name")]
        public string Email { get; set; }
        
        // [Required(ErrorMessage = "Not specified name")]
        // public string Name { get; set; }

        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}