namespace UniwayBackend.Config
{
    public interface IConfigurationLib
    {
        int SuccessCode { get; } // 200
        string SuccessMessage { get; }

        int CreatedCode { get; } // 201
        string CreatedMessage { get; }

        int AcceptedCode { get; } // 202
        string AcceptedMessage { get; }

        int NoContentCode { get; } // 204
        string NoContentMessage { get; }

        int NotFoundCode { get; } // 404
        string NotFoundMessage { get; }
        string NotFoundMessageUpdate { get; }
        string NotFoundMessageDelete { get; }

        int BadRequestCode { get; } // 400
        string BadRequestMessage { get; }

        int UnauthorizedCode { get; } // 401
        string UnauthorizedMessage { get; }

        int ForbiddenCode { get; } // 403
        string ForbiddenMessage { get; }

        int MethodNotAllowedCode { get; } // 405
        string MethodNotAllowedMessage { get; }

        int ConflictCode { get; } // 409
        string ConflictMessage { get; }

        int InternalServerErrorCode { get; } // 500
        string InternalServerErrorMessage { get; }

        int NotImplementedCode { get; } // 501
        string NotImplementedMessage { get; }

        int BadGatewayCode { get; } // 502
        string BadGatewayMessage { get; }

        int ServiceUnavailableCode { get; } // 503
        string ServiceUnavailableMessage { get; }

        int GatewayTimeoutCode { get; } // 504
        string GatewayTimeoutMessage { get; }

        //int UnspecifiedErrorCode { get; }
        //string UnspecifiedErrorMessage { get; }

        // Puedes agregar más según sea necesario...
    }

}

