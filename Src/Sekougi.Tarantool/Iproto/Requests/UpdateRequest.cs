using System.Runtime.CompilerServices;
using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Iproto.UpdateOperations;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class UpdateRequest<TKey> : RequestBase where TKey : ITuple
    {
        private const int IPROTO_SPACE_ID_KEY = 0x10;
        private const int IPROTO_INDEX_ID_KEY = 0x11;
        private const int IPROTO_KEY_KEY = 0x20;
        private const int IPROTO_TUPLE_KEY = 0x21;

        private readonly uint _spaceId;
        private readonly uint _indexId;
        private readonly TKey _key;
        private readonly IUpdateOperation[] _updateOperations;
        
        public override CommandE Code => CommandE.Update;


        public UpdateRequest(uint spaceId, uint indexId, TKey key, params IUpdateOperation[] updateOperations)
        {
            _spaceId = spaceId;
            _indexId = indexId;
            _key = key;
            _updateOperations = updateOperations;
        }
        
        protected override void SerializeBody(MessagePackWriter writer)
        {
            writer.WriteDictionaryLength(4);
            
            writer.Write(IPROTO_SPACE_ID_KEY);
            writer.Write(_spaceId);
            
            writer.Write(IPROTO_INDEX_ID_KEY);
            writer.Write(_indexId);
            
            writer.Write(IPROTO_KEY_KEY);
            MessagePackSerializersRepository.Get<TKey>().Serialize(_key, writer);
            
            // TODO: need to add normal serializer for UpdateOperation, but i have no time
            writer.Write(IPROTO_TUPLE_KEY);
            writer.WriteArrayLength(_updateOperations.Length);
            foreach (var operation in _updateOperations)
            {
                operation.Serialize(writer);
            }
        }
    }
}