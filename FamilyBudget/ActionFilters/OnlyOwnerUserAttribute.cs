using FamilyBudget.Entities;
using FamilyBudget.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FamilyBudget.ActionFilters { 

    public class OnlyOwnerUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //int loggedUserId = context.HttpContext.Session.GetObject<User>("loggedUser").Id;
            //var routeId = context.HttpContext.Request.Headers;

            //context.Result = new RedirectResult("/Home/Index");

            //if (loggedUserId != routeId && routeId != 0)
            //{
            //    context.Result = new RedirectResult("/Home/Index");
            //}
        }
    }
}
