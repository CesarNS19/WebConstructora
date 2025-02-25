using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConstructoraWeb.Models.Filters
{
    public class Session : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userEmail = session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("Auth", "Account", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
