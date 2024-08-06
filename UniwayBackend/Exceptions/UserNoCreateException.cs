using System.Runtime.Serialization;

namespace UniwayBackend.Exceptions
{

    [Serializable] // Marca la clase como serializable, lo cual es necesario para permitir la serialización.
    public class UserNoCreateException : ApplicationException // Heredamos de ApplicationException para indicar que es una excepción específica de la aplicación.
    {
        // Propiedad que almacena el nombre del rol asociado con la excepción.
        public string RoleName { get; private set; }

        // Constructor que acepta solo un mensaje de error.
        public UserNoCreateException(string message) : base($"Error al crear el usuario: {message}") { }

        // Constructor que acepta un mensaje de error y el nombre del rol.
        public UserNoCreateException(string message, string roleName) : base($"Error al crear el usuario: {message}")
        {
            this.RoleName = roleName; // Asigna el nombre del rol a la propiedad RoleName.
        }

        // Constructor de serialización (necesario para soportar la serialización).
        // Se llama cuando la excepción se está deserializando.
        protected UserNoCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                RoleName = info.GetString("RoleName"); // Recupera el valor de RoleName de la información de serialización.
            }
        }

        // Implementación para la serialización de la excepción.
        // Se llama cuando la excepción se está serializando.
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context); // Llama al método base para serializar la información de la excepción base.
            if (info != null)
            {
                info.AddValue("RoleName", RoleName); // Agrega el valor de RoleName a la información de serialización.
            }
        }
    }

}
