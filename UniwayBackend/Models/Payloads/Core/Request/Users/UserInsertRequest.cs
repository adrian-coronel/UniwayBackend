using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Request.Users
{
    public class UserInsertRequest
    {
        [DefaultValue("xxxx@gmail.com")]
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Email debe ser una dirección de correo electrónico válida.")]
        public required string Email { get; set; }

        [DefaultValue("xxxx")]
        [Required(ErrorMessage = "El campo Password es obligatorio.")]
        [MinLength(4, ErrorMessage = "La longitud mínima de la contraseña es de 4 caracteres.")]
        public required string Password { get; set; }

        [DefaultValue("2")]
        [Required(ErrorMessage = "El campo RoleId es obligatorio.")]
        public required short RoleId { get; set; }
    }
}
