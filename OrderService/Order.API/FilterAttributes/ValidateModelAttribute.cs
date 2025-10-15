using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Order.API.FilterAttributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors.Select(error => new
                    {
                        Code = e.Key,
                        Message = error.ErrorMessage
                    }))
                    .ToArray();

                context.Result = new BadRequestObjectResult(new { errors });
            }
        }
    }
}
