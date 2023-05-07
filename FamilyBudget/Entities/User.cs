using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Entities
{
    public class User
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Email { get; set; }
        
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public DateTime RegisteredTime { get; set; }

        [Required]
        public bool Sex { get; set; }

    }
}
