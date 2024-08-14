namespace UniwayBackend.Models.Payloads.Base.Response
{
    public class MessageResponseBuilder<TEntity> where TEntity : class
    {
        private MessageResponse<TEntity> _messageResponse;

        public MessageResponseBuilder()
        {
            _messageResponse = new MessageResponse<TEntity>();
        }

        public MessageResponseBuilder<TEntity> Code(int Code)
        {
            _messageResponse.Code = Code;
            return this;
        }
        public MessageResponseBuilder<TEntity> Message(string? Message)
        {
            _messageResponse.Message = Message;
            return this;
        }

        public MessageResponseBuilder<TEntity> Object(TEntity? Object)
        {
            _messageResponse.Object = Object;
            return this;
        }

        public MessageResponseBuilder<TEntity> List(IEnumerable<TEntity>? List)
        {
            _messageResponse.List = List;
            return this;
        }

        public MessageResponseBuilder<TEntity> IsResultList(bool IsResultList = false)
        {
            _messageResponse.IsResultList = IsResultList;
            return this;
        }

        public MessageResponseBuilder<TEntity> FuntionalErrors(List<string>? FunctionalErrors)
        {
            _messageResponse.FunctionalErrors = FunctionalErrors;
            return this;
        }

        public MessageResponse<TEntity> Build()
        {
            return _messageResponse;
        }
    }
}
