using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public abstract class RequestBase
    {
        private Header Header;
        
        public virtual CommandE Code { get; }

        public int SyncId
        {
            get => Header.SyncId;
            set => Header.SetSyncId(value);
        }

        public int Schema
        {
            get => Header.SchemaVersion;
            set => Header.SetSchemaVersion(value);
        }


        protected RequestBase()
        {
            Header.SetCode(Code);
        }
        
        public void Serialize(MessagePackWriter writer)
        {
            Header.Serialize(writer);
            SerializeBody(writer);
        }

        protected abstract void SerializeBody(MessagePackWriter writer);
    }
}