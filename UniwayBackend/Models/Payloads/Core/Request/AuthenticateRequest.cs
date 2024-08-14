using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UniwayBackend.Models.Payloads.Core.Request
{
    public class AuthenticateRequest
    {
        [DefaultValue("xxxx@gmail.com")]
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Email debe ser una dirección de correo electrónico válida.")]
        public required string Email { get; set; }

        [DefaultValue("xxxxxx")]
        [Required(ErrorMessage = "El campo Password es obligatorio.")]
        public required string Password { get; set; }
    }

    public class RegisterRequest
    {
        // User credentials
        [DefaultValue("xxxx@gmail.com")]
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Email debe ser una dirección de correo electrónico válida.")]
        public required string Email { get; set; }

        [DefaultValue("xxxx")]
        [Required(ErrorMessage = "El campo Password es obligatorio.")]
        [MinLength(4, ErrorMessage = "La longitud mínima de la contraseña es de 4 caracteres.")]
        public required string Password { get; set; }


        [DefaultValue("2")]
        [Required(ErrorMessage = "El campo Role es obligatorio.")]
        public required short RoleId { get; set; }

        // Atributos cliente, tecnico
        [DefaultValue("user01")]
        [Required(ErrorMessage = "El campo Name es obligatorio.")]
        public required string Name { get; set; }

        [DefaultValue("Torres")]
        [Required(ErrorMessage = "El campo FatherLastname es obligatorio.")]
        public required string FatherLastname { get; set; }

        [DefaultValue("Torres")]
        [Required(ErrorMessage = "El campo MotherLastname es obligatorio.")]
        public required string MotherLastname { get; set; }

        [DefaultValue("xxxxxxx")]
        [Required(ErrorMessage = "El campo Dni es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El campo Dni debe contener exactamente 8 dígitos.")]
        public required string Dni { get; set; }

        [Required(ErrorMessage = "El campo BirthDate es obligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "El campo BirthDate debe ser una fecha válida.")]
        public required DateTime BirthDate { get; set; }


        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
    }
}
