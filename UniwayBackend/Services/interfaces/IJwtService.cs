using UniwayBackend.Models.Entities;

namespace UniwayBackend.Services.interfaces
{
    public interface IJwtService
    {

        /// <summary>
        /// Genera un token JWT para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el cual se generará el token.</param>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado es el token JWT generado como una cadena.</returns>
        Task<string> GenerateJwtToken(User user);
    }
}
