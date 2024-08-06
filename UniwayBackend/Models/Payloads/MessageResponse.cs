namespace UniwayBackend.Models.Payloads
{
    public class MessageResponse<TEntity> where TEntity : class
    {
        public int Code {  get; set; }
        public string? Message { get; set; }
        public TEntity? Object { get; set; }
        public IEnumerable<TEntity>? List { get; set; }
        public bool IsResultList { get; set; } = false;
        public List<string>? FunctionalErrors { get; set; }


    }
}
