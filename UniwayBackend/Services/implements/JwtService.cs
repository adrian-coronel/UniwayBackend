using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniwayBackend.Config;
using UniwayBackend.Models.Entities;
using UniwayBackend.Services.interfaces;

namespace UniwayBackend.Services.implements
{
    public class JwtService : IJwtService
    {

        private readonly AppSettings _appSettings;

        public JwtService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Genera un token JWT para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se generará el token.</param>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado es el token JWT generado como una cadena.</returns>
        public async Task<string> GenerateJwtToken(User user)
        {
            // Genera un token que es válido por 7 días
            var tokenHandler = new JwtSecurityTokenHandler(); // Instancia para manejar tokens JWT

            // Genera el token de forma asíncrona para no bloquear el subproceso principal
            var token = await Task.Run(() =>
            {
                // Obtiene la clave secreta del appSettings y la convierte en un arreglo de bytes
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                // Define los parámetros del token JWT en un objeto SecurityTokenDescriptor
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // Establece la identidad del token (Subject) con el ID del usuario como una reclamación
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),

                    // Establece la fecha y hora de expiración del token (7 días desde la fecha y hora actual)
                    Expires = DateTime.UtcNow.AddDays(7),

                    // Establece las credenciales de firma del token utilizando la clave secreta y el algoritmo de firma
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                // Crea el token JWT utilizando el descriptor de token
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            // Escribe el token JWT como una cadena y lo devuelve
            return tokenHandler.WriteToken(token);
        }
    }
}
