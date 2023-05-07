using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FamilyBudget.ExtensionMethods;
using FamilyBudget.Entities;

namespace FamilyBudget.ActionFilters
{
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                context.Result = new RedirectResult("/User/Save?IsRegister=false");
            }
        }
    }
}
