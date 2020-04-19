using System.Runtime.CompilerServices;
using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class InsertRequest<T> : RequestBase where T : ITuple
    {
        private const int IPROTO_SPACE_ID_KEY = 0x10;
        private const int IPROTO_TUPLE_KEY = 0x21;

        private readonly uint _spaceId;
        private readonly T _dataToInsert;
        
        public override CommandE Code => CommandE.Insert;

        
        public InsertRequest(uint spaceId,  T dataToInsert)
        {
            _spaceId = spaceId;
            _dataToInsert = dataToInsert;
        }
        
        protected override void SerializeBody(MessagePackWriter writer)
        {
            writer.WriteDictionaryLength(2);
            
            writer.Write(IPROTO_SPACE_ID_KEY);
            writer.Write(_spaceId);
            
            writer.Write(IPROTO_TUPLE_KEY);
            MessagePackSerializersRepository.Get<T>().Serialize(_dataToInsert, writer);
        }
    }
}