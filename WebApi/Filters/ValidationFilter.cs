using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Dtos;

namespace WebApi.Filters;

public class ValidationFilter : ActionFilterAttribute
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);
            context.Result = new ObjectResult(new ErrorDto(errors.ToList(),true))
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}