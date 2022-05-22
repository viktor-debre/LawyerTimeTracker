using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LawyerTimeTracker.Models
{
    public class Role
    {
        [Key]
        public string Name { get; set; }
    }
}