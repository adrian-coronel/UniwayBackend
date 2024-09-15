

namespace UniwayBackend.Models.Payloads.Core.Response
{
    public class AuthenticateResponse<TEntity> where TEntity : class
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public TEntity? Data { get; set; }
        public string Token { get; set; }
        public List<string>? FunctionalErrors { get; set; }

    }
}
