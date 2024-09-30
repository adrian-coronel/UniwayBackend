using Azure;
using Microsoft.AspNetCore.Authentication;
using UniwayBackend.Config;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Models.Payloads.Base.Response
{
    public class UtilitariesResponse<T> where T : class
    {

        private readonly IConfigurationLib _configuration;


        // STATUS CODE
        public UtilitariesResponse(IConfigurationLib configuration)
        {
            _configuration = configuration;
        }

        public MessageResponse<T> setResponseBaseForOk()
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.SuccessCode)
                .Message(_configuration.SuccessMessage)
                .Build();
        }
        public MessageResponse<T> setResponseBaseForNotFount()
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.NotFoundCode)
                .Message(_configuration.NotFoundMessage)
                .Build();
        }
        public MessageResponse<T> setResponseBaseNotFoundForDelete()
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.NotFoundCode)
                .Message(_configuration.NotFoundMessageDelete)
                .Build();
        }
        public MessageResponse<T> setResponseBaseNotFoundForUpdate()
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.NotFoundCode)
                .Message(_configuration.NotFoundMessageUpdate)
                .Build();
        }
        public MessageResponse<T> setResponseBaseForInternalServerError()
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.InternalServerErrorCode)
                .Message(_configuration.InternalServerErrorMessage)
                .Build();
        }
        public MessageResponse<T> setResponseBaseForBadRequest(string message = null)
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.BadRequestCode)
                .Message(message ?? _configuration.BadRequestMessage)
                .Build();
        }

        public AuthenticateResponse<T> setResponseBaseForNotFoundAuthenticate()
        {
            return new AuthenticateResponseBuilder<T>()
                .Code(_configuration.NotFoundCode)
                .Message(_configuration.NotFoundMessage)
                .Build();
        }
        public AuthenticateResponse<T> setResponseBaseForInternalErrorAuth(string message)
        {
            return new AuthenticateResponseBuilder<T>()
                .Code(_configuration.InternalServerErrorCode)
                .Message(message)
                .Build();
        }
        


        // DATA
        public MessageResponse<T> setResponseBaseForList(IEnumerable<T> list)
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.SuccessCode)
                .Message(_configuration.SuccessMessage)
                .List(list)
                .IsResultList(true)
                .Build();
        }

        public MessageResponse<T> setResponseBaseForObject(T obj)
        {
            return new MessageResponseBuilder<T>()
                .Code(_configuration.SuccessCode)
                .Message(_configuration.SuccessMessage)
                .Object(obj)
                .Build();
        }
        public MessageResponse<T> setResponseBasePersonalized(int code, string message, T? @object, IEnumerable<T>? list, bool isResultList, List<string>? functionalErrors)
        {
            return new MessageResponseBuilder<T>()
                .Code(code)
                .Message(message)
                .Object(@object)
                .List(list)
                .IsResultList(isResultList)
                .FuntionalErrors(functionalErrors)
                .Build();
        }
        public AuthenticateResponse<T> setResponseBaseForToken(string token, T? data = null)
        {
            return new AuthenticateResponseBuilder<T>()
                .Code(_configuration.SuccessCode)
                .Message(_configuration.SuccessMessage)
                .Token(token)
                .Data(data)
                .Build();
        }


        // EXCEPTION
        public MessageResponse<T> setResponseBaseForException(Exception ex, List<string>? FunctionalErrors = null)
        {
            if (ex is TimeoutException)
            {
                return new MessageResponseBuilder<T>()
                    .Code(_configuration.InternalServerErrorCode)
                    .Message(_configuration.InternalServerErrorMessage)
                    .FuntionalErrors(FunctionalErrors)
                    .Build();
            }
            else if (ex is HttpRequestException)
            {
                return new MessageResponseBuilder<T>()
                    .Code(_configuration.GatewayTimeoutCode)
                    .Message(_configuration.GatewayTimeoutMessage)
                    .FuntionalErrors(FunctionalErrors)
                    .Build();
            }
            else if (ex is AuthenticationFailureException)
            {
                return new MessageResponseBuilder<T>()
                    .Code(_configuration.UnauthorizedCode)
                    .Message(_configuration.UnauthorizedMessage)
                    .FuntionalErrors(FunctionalErrors)
                    .Build();
            }
            else
            {
                return new MessageResponseBuilder<T>()
                    .Code(_configuration.InternalServerErrorCode)
                    .Message(_configuration.InternalServerErrorMessage)
                    .FuntionalErrors(FunctionalErrors)
                    .Build();
            }
        }
        public AuthenticateResponse<T> setResponseBaseForAuthException(Exception ex, List<string>? FunctionalErrors = null)
        {
            if (ex is TimeoutException)
            {
                return new AuthenticateResponseBuilder<T>()
                    .Code(_configuration.InternalServerErrorCode)
                    .Message(_configuration.InternalServerErrorMessage)
                    .FunctionalErrors(FunctionalErrors)
                    .Build();
            }
            else if (ex is HttpRequestException)
            {
                return new AuthenticateResponseBuilder<T>()
                    .Code(_configuration.GatewayTimeoutCode)
                    .Message(_configuration.GatewayTimeoutMessage)
                    .FunctionalErrors(FunctionalErrors)
                    .Build();
            }
            else if (ex is AuthenticationFailureException)
            {
                return new AuthenticateResponseBuilder<T>()
                    .Code(_configuration.UnauthorizedCode)
                    .Message(_configuration.UnauthorizedMessage)
                    .FunctionalErrors(FunctionalErrors)
                    .Build();
            }
            else
            {
                return new AuthenticateResponseBuilder<T>()
                    .Code(_configuration.InternalServerErrorCode)
                    .Message(ex.Message)
                    .FunctionalErrors(FunctionalErrors)
                    .Build();
            }
        }


    }
}
