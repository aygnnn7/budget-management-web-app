using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using static FamilyBudget.ProjectEnums;

namespace FamilyBudget.Models.User
{
    public class EditVM
    {
        public int Id { get; set; }

        [Display(Prompt = "Email")]
        [MaxLength(30, ErrorMessage = "The email must be maximum 30 characters.")]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Prompt = "Name")]
        [MinLength(3, ErrorMessage = "The name must be minimum 3 characters.")]
        [MaxLength(15, ErrorMessage = "The name must be maximum 15 characters.")]
        [Required]
        public string? Name { get; set; }

        [Display(Prompt = "Sex")]
        [Required]
        public Sex Sex { get; set; }
    }
}
