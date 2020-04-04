using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto.Responses
{
    public class OkResponse : ResponseBase
    {
        protected override void DeserializeBody(MessagePackReader reader)
        {
            reader.ReadDictionaryLength();
        }
    }
}