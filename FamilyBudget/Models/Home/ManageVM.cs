using Castle.Components.DictionaryAdapter;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using FamilyBudget.Entities;
namespace FamilyBudget.Models.Home
{
    public class ManageVM
    {
        public class NotZeroAttribute : ValidationAttribute
        {
            public override bool IsValid(object value) => (decimal)value != 0;
        }

        public int Id { get; set; }

        [Display(Prompt = "Amount")]
        [Required(ErrorMessage = "Please enter an amount of money!")]
        [Range(0, (Double)Decimal.MaxValue, ErrorMessage = "The field must be a number bigger than 0.")]
        [NotZero(ErrorMessage = "The value must be not equal to zero!"),]
        public decimal Value { get; set; }


        [Display(Prompt = "Category(optional)")]
        [Required(ErrorMessage = "Please select a category!")]
        public string? Category { get; set; }

        [Display(Prompt = "Type")]
        public string? Type { get; set; }

        [Display(Prompt = "Repeat Day Of The Month")]
        public int RepeatDay { get; set; }


        [Display(Prompt = "Description")]
        [MinLength(5, ErrorMessage = "The description must be minimum 5 characters.")]
        [MaxLength(150, ErrorMessage = "The description must be maximum 150 characters.")]
        [Required]
        public string? Description { get; set; }
        
        public DateTime CreatedTime { get; set; }

        public bool IsRepetitive { get; set; }

        //is create or edit
        public bool IsCreate { get; set; }

    }
}