using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Models.User
{
    public class LoginVM
    {

        [Display(Prompt = "Email")]
        [MaxLength(30, ErrorMessage = "The email must be maximum 30 characters.")]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }


        [Display(Prompt = "Password")]
        [MinLength(6, ErrorMessage = "The password must be minimum 6 characters.")]
        [MaxLength(15, ErrorMessage = "The password must be maximum 15 characters.")]
        [Required]
        public string? Password { get; set; }
    }
}
