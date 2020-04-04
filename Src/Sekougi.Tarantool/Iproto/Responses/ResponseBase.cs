using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto.Responses
{
    public abstract class ResponseBase
    {
        public int SyncId { get; private set; }

        
        public void Deserialize(MessagePackReader reader, int syncId)
        {
            SyncId = syncId;
            DeserializeBody(reader);
        }

        protected abstract void DeserializeBody(MessagePackReader reader);
    }
}