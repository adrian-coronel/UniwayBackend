using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Helpers
{
    /// <summary>
    /// EN DESHUSO
    /// </summary>
    public class JwtMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings; // Contiene la firma secreta

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            // Extraemos el token JWT del encabezado de autorización
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Si el token JWT existe se ejecuta attachUserToContext
            if (token != null)
                await attachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler(); // Crea un manejador de tokens JWT.

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret); // Obt. llave secreta y la convertirmos en bytes

                // Valida el token JWT utilizando los parámetros de validación especificados.
                // Se configuran para validar la firma del emisor y el tiempo de expiración del token.
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Se debe validar la clave Secrect
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Aquí se especifica la clave de firma del emisor del token JWT.
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Convierte el token validado en un objeto JwtSecurityToken.
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Obtiene el ID de usuario del token.
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                MessageResponse<User> user = await userService.GetById(userId);

                // Adjunta el usuario al contexto de la solicitud utilizando el servicio de usuario y el ID de usuario.
                context.Items["User"] = user;
            }
            catch
            {
                //Do nothing if JWT validation fails
                // user is not attached to context so the request won't have access to secure routes
            }
        }
    }
}
