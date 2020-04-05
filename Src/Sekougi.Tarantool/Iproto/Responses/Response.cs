using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto.Responses
{
    public class Response
    {
        public int SyncId { get; protected set; }
        
        
        public void Initialize(int syncId, MessagePackReader reader)
        {
            SyncId = syncId;
            ReadBody(reader);
        }

        protected virtual void ReadBody(MessagePackReader reader)
        {
            reader.ReadDictionaryLength();
        }
    }
}