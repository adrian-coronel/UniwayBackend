using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Models.Payloads.Base.Response
{
    public class AuthenticateResponseBuilder<TEntity> where TEntity : class
    {
        private AuthenticateResponse<TEntity> response;

        public AuthenticateResponseBuilder()
        {
            response = new AuthenticateResponse<TEntity>();
        }

        public AuthenticateResponseBuilder<TEntity> Code(int Code)
        {
            response.Code = Code;
            return this;
        }
        public AuthenticateResponseBuilder<TEntity> Message(string Message)
        {
            response.Message = Message;
            return this;
        }
        public AuthenticateResponseBuilder<TEntity> Token(string Token)
        {
            response.Token = Token;
            return this;
        }
        public AuthenticateResponseBuilder<TEntity> Data(TEntity? Data)
        {
            response.Data = Data;
            return this;
        }

        public AuthenticateResponseBuilder<TEntity> FunctionalErrors(List<string>? FunctionalErrors)
        {
            response.FunctionalErrors = FunctionalErrors;
            return this;
        }

        public AuthenticateResponse<TEntity> Build()
        {
            return response;
        }

    }
}
