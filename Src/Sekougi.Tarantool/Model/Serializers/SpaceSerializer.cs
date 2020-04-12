using System.Text;
using Sekougi.MessagePack;
using Sekougi.MessagePack.Serializers;



namespace Sekougi.Tarantool.Model.Serializers
{
    public class SpaceSerializer : MessagePackSerializer<Space>
    {
        private const int SPACE_LENGTH = 7;
        
        private readonly MessagePackSerializer<FlagsInfo> _flagsSerializer;
        private readonly MessagePackSerializer<FieldMetaInfo[]> _fieldsSerializer;
        
        
        public SpaceSerializer()
        {
            _flagsSerializer = MessagePackSerializersRepository.Get<FlagsInfo>();
            _fieldsSerializer = MessagePackSerializersRepository.Get<FieldMetaInfo[]>();
        }
        
        public override void Serialize(Space value, MessagePackWriter writer)
        {
            writer.WriteArrayHeader(SPACE_LENGTH);
            
            writer.Write(value.Id);
            writer.Write(value.OwnerId);
            writer.Write(value.Name, Encoding.UTF8);
            writer.Write(value.Engine, Encoding.UTF8);
            writer.Write(value.FieldsCount);
            
            _flagsSerializer.Serialize(value.Flags, writer);
            _fieldsSerializer.Serialize(value.Fields, writer);
        }

        public override Space Deserialize(MessagePackReader reader)
        {
            reader.ReadArrayLength();
            
            var id = reader.ReadUint();
            var ownerId = reader.ReadUint();
            var name = reader.ReadString(Encoding.UTF8);
            var engine = reader.ReadString(Encoding.UTF8);
            var fieldsCount = reader.ReadInt();
            var flags = _flagsSerializer.Deserialize(reader);
            var fields = _fieldsSerializer.Deserialize(reader);

            var space = new Space(id, ownerId, name, engine, fieldsCount, flags, fields);


            return space;
        }
    }
}