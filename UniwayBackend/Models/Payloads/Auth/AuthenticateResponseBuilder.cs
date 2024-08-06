using UniwayBackend.Models.Entities;

namespace UniwayBackend.Models.Payloads.Auth
{
    public class AuthenticateResponseBuilder<TEntity> where TEntity : class
    {
        private AuthenticateResponse<TEntity> response;

        public AuthenticateResponseBuilder()
        {
            this.response = new AuthenticateResponse<TEntity>();
        }

        public AuthenticateResponseBuilder<TEntity> Code(int Code)
        {
            this.response.Code = Code;
            return this;
        }
        public AuthenticateResponseBuilder<TEntity> Message(string Message)
        {
            this.response.Message = Message;
            return this;
        }
        public AuthenticateResponseBuilder<TEntity> Token(string Token)
        {
            this.response.Token = Token;
            return this;
        }
        public AuthenticateResponseBuilder<TEntity> Data(TEntity? Data)
        {
            this.response.Data = Data;
            return this;
        }

        public AuthenticateResponseBuilder<TEntity> FunctionalErrors(List<string>? FunctionalErrors)
        {
            this.response.FunctionalErrors = FunctionalErrors;
            return this;
        }

        public AuthenticateResponse<TEntity> Build()
        {
            return this.response;
        }

    }
}
