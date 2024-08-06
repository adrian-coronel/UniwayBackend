namespace UniwayBackend.Config
{
    public class ConfigurationLib : IConfigurationLib
    {
        public int SuccessCode => 200;
        public string SuccessMessage => "La operación se realizó exitosamente";

        public int CreatedCode => 201;
        public string CreatedMessage => "El recurso se ha creado correctamente";

        public int AcceptedCode => 202;
        public string AcceptedMessage => "La solicitud ha sido aceptada para procesamiento";

        public int NoContentCode => 204;
        public string NoContentMessage => "No se encontraron datos";

        public int NotFoundCode => 404;
        public string NotFoundMessage => "No se encontró el recurso solicitado";
        public string NotFoundMessageUpdate => "No se encontró el recurso solicitado para actualizar";
        public string NotFoundMessageDelete => "No se encontró el recurso solicitado para eliminar";

        public int BadRequestCode => 400;
        public string BadRequestMessage => "La solicitud no es válida";

        public int UnauthorizedCode => 401;
        public string UnauthorizedMessage => "No autorizado para acceder al recurso";

        public int ForbiddenCode => 403;
        public string ForbiddenMessage => "No tiene permisos para acceder al recurso";

        public int MethodNotAllowedCode => 405;
        public string MethodNotAllowedMessage => "Método de solicitud no permitido";

        public int ConflictCode => 409;
        public string ConflictMessage => "Conflicto al procesar la solicitud";

        public int InternalServerErrorCode => 500;
        public string InternalServerErrorMessage => "Error interno del servidor";

        public int NotImplementedCode => 501;
        public string NotImplementedMessage => "Funcionalidad solicitada no implementada";

        public int BadGatewayCode => 502;
        public string BadGatewayMessage => "Puerta de enlace incorrecta";

        public int ServiceUnavailableCode => 503;
        public string ServiceUnavailableMessage => "Servicio no disponible";

        public int GatewayTimeoutCode => 504;
        public string GatewayTimeoutMessage => "Tiempo de espera de la puerta de enlace agotado";


        //public int UnspecifiedErrorCode => -200;
        //public string UnspecifiedErrorMessage => throw new NotImplementedException();
    }

}
