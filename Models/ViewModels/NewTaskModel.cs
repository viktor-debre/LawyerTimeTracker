using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.Models.ViewModels
{
    public class NewTaskModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Not specified title")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Not specified type of task")]
        public string TypeOfTask { get; set; }
    }
}