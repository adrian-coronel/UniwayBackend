using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads;

namespace UniwayBackend.Helpers
{
    // Define el uso de este atributo en clases y métodos
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] 
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter // Define la clase AuthorizeAttribute que hereda de Attribute e implementa IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context) // Método que se ejecuta durante la autorización de la solicitud
        {
            // Obtiene el usuario adjunto al contexto de la solicitud
            var response = (MessageResponse<User>?)context.HttpContext.Items["User"]; 

            if (response?.Object == null) // Si no hay usuario en el contexto
            {
                // Establece el resultado de la autorización como no autorizado
                context.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }

}
