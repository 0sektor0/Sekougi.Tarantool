using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;


namespace Sekougi.Tarantool.Iproto
{
    public struct Header
    {
        private const int IPROTO_REQUEST_TYPE_KEY = 0x00;
        private const int IPROTO_SCHEMA_VERSION_KEY = 0x05;
        private const int IPROTO_SYNC_KEY = 0x01;

        public int SyncId;
        public int SchemaVersion;
        public CommandE Code;

        
        public void SetCode(CommandE code)
        {
            Code = code;
        }
        
        public void SetSyncId(int id)
        {
            SyncId = id;
        }
        
        public void SetSchemaVersion(int schema)
        {
            SchemaVersion = schema;
        }
        
        public void Serialize(MessagePackWriter writer)
        {
            if (SchemaVersion > 0)
            {
                writer.WriteDictionaryHeader(3);
                writer.Write(IPROTO_SCHEMA_VERSION_KEY);
                writer.Write(SchemaVersion);
            }
            else
            {
                writer.WriteDictionaryHeader(2);
            }
            
            writer.Write(IPROTO_REQUEST_TYPE_KEY);
            writer.Write((int) Code);
            
            writer.Write(IPROTO_SYNC_KEY);
            writer.Write(SyncId);
        }

        public void Deserialize(MessagePackReader reader)
        {
            var dictionaryLength = reader.ReadDictionaryLength();
            for (var i = 0; i < dictionaryLength; i++)
            {
                var key = reader.ReadUint();
                switch (key)
                {
                    case IPROTO_REQUEST_TYPE_KEY:
                        Code = (CommandE) reader.ReadUint();
                        break;
                    
                    case IPROTO_SYNC_KEY:
                        SyncId = (int) reader.ReadUint();
                        break;
                    
                    case IPROTO_SCHEMA_VERSION_KEY:
                        SchemaVersion = (int) reader.ReadUint();
                        break;
                }
            }
        }
    }
}