using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using static FamilyBudget.ProjectEnums;

namespace FamilyBudget.Models.User
{
  
    public class SaveUserVM
    {
        public class PasswordsAreEqual : ValidationAttribute
        {
            public override bool IsValid(object value) => (bool)value;

        }

        [PasswordsAreEqual(ErrorMessage = "Passwords must be equal!")]
        public bool ArePasswordsEqual { get { return Password == PasswordAgain; } }
       
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

        [Display(Prompt = "Password Again")]
        [Required]
        public string? PasswordAgain { get; set; }
        
        [Display(Prompt = "Name")]
        [MinLength(3, ErrorMessage = "The name must be minimum 3 characters.")]
        [MaxLength(15, ErrorMessage = "The name must be maximum 15 characters.")]
        [Required]
        public string? Name { get; set; }

        [Display(Prompt = "Sex")]
        [Required]
        public Sex Sex { get; set; }
        public bool IsRegister { get; set; }
        
        
    }
   
}

