using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class SelectRequest : RequestBase
    {
        private const int IPROTO_SPACE_ID_KEY = 0x10;
        private const int IPROTO_INDEX_ID_KEY = 0x11;
        private const int IPROTO_LIMIT_KEY    = 0x12;
        private const int IPROTO_OFFSET_KEY   = 0x13;
        private const int IPROTO_ITERATOR_KEY = 0x14;
        private const int IPROTO_KYE_KEY      = 0x20;
        
        private readonly uint _spaceId;
        private readonly uint _limit;
        private readonly uint[] _key;
        private readonly int _indexId;
        private readonly int _offset;
        private readonly IteratorE _iterator;
        
        public override CommandE Code => CommandE.Select;


        // TODO: use ReadonlySpan
        public SelectRequest(uint spaceId, int indexId, uint limit, int offset, IteratorE iterator, params uint[] key)
        {
            _spaceId = spaceId;
            _indexId = indexId;
            _limit = limit;
            _offset = offset;
            _iterator = iterator;
            _key = key;
        }
        
        public SelectRequest(uint spaceId, int indexId, IteratorE iterator, params uint[] key) 
            : this(spaceId, indexId, uint.MaxValue, 0, iterator, key)
        {
            
        }
        
        protected override void SerializeBody(MessagePackWriter writer)
        {
            writer.WriteDictionaryLength(6);
            
            writer.Write(IPROTO_SPACE_ID_KEY);
            writer.Write(_spaceId);
            
            writer.Write(IPROTO_INDEX_ID_KEY);
            writer.Write(_indexId);
            
            writer.Write(IPROTO_LIMIT_KEY);
            writer.Write(_limit);
            
            writer.Write(IPROTO_OFFSET_KEY);
            writer.Write(_offset);
            
            writer.Write(IPROTO_ITERATOR_KEY);
            writer.Write((uint)_iterator);

            writer.Write(IPROTO_KYE_KEY);
            MessagePackSerializersRepository.Get<uint[]>().Serialize(_key, writer);
        }
    }
}