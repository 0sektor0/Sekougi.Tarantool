using System;
using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto.Responses
{
    public class DataResponse : ResponseBase
    {
        protected override void DeserializeBody(MessagePackReader reader)
        {
            reader.ReadDictionaryLength();
            throw new NotImplementedException();
        }
    }
}