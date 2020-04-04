using System.Text;
using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto.Responses
{
    public class ErrorResponse : ResponseBase
    {
        public string Message { get; private set; }


        protected override void DeserializeBody(MessagePackReader reader)
        {
            reader.ReadDictionaryLength();
            reader.ReadUint();
            Message = reader.ReadString(Encoding.UTF8);
        }
    }
}