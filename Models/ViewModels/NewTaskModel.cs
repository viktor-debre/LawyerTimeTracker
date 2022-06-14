using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.ViewModels
{
    public class NewTaskModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Not specified title")]
        [StringLength(50, MinimumLength = 6 ,ErrorMessage = "Title of task must contain {2}-{1} characters")]
        public string Title { get; set; }
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Not specified type of task")]
        [StringLength(20, MinimumLength = 3 ,ErrorMessage = "Title of task must contain {2}-{1} characters")]
        public string TypeOfTask { get; set; }
    }
}