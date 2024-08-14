using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;

namespace UniwayBackend.Helpers.Filters
{
    public class ModelStateInvalidFilter : IActionFilter
    {
        // Este método se ejecuta después de que se ha ejecutado la acción del controlador.
        public void OnActionExecuted(ActionExecutedContext context){ }

        // Este método se ejecuta antes de que se ejecute la acción del controlador. 
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new AuthenticateResponseBuilder<User>()
                    .Code(400)
                    .Message("Uno o más errores de validación ocurrieron")
                    .FunctionalErrors(new List<string>())
                    .Build();

                foreach (var key in context.ModelState.Keys)
                {
                    foreach (var error in context.ModelState[key].Errors)
                    {
                        response.FunctionalErrors.Add(error.ErrorMessage);
                    }
                }

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
