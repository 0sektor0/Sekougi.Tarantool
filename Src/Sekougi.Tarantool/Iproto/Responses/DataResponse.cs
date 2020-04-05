using Sekougi.MessagePack;
using Sekougi.MessagePack.Serializers;



namespace Sekougi.Tarantool.Iproto.Responses
{
    public class DataResponse<T> : Response
    {
        private MessagePackSerializer<T> _dataSerializer;
        
        public T Data { get; private set; }
        

        public DataResponse()
        {
            _dataSerializer = MessagePackSerializersRepository.Get<T>();
        }
        
        protected override void ReadBody(MessagePackReader reader)
        {
            Data = _dataSerializer.Deserialize(reader);
        }
    }
}