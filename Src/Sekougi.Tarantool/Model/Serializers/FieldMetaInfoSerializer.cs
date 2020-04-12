using System.Text;
using Sekougi.MessagePack;
using Sekougi.MessagePack.Serializers;
using Sekougi.Tarantool.Exceptions;



namespace Sekougi.Tarantool.Model.Serializers
{
    public class FieldMetaInfoSerializer : MessagePackSerializer<FieldMetaInfo>
    {
        private const int FIELD_LENGTH = 2;
        private const int NULLABLE_FIELD_LENGTH = 3;
        
        
        public override void Serialize(FieldMetaInfo value, MessagePackWriter writer)
        {
            writer.WriteDictionaryHeader(value.IsNullable ? NULLABLE_FIELD_LENGTH : FIELD_LENGTH);
            
            writer.Write("name", Encoding.UTF8);
            writer.Write(value.Name, Encoding.UTF8);
            
            writer.Write("type", Encoding.UTF8);
            writer.Write(value.Type, Encoding.UTF8);
            
            if (!value.IsNullable) return;
            
            writer.Write("is_nullable", Encoding.UTF8);
            writer.Write(true);
        }

        public override FieldMetaInfo Deserialize(MessagePackReader reader)
        {
            var length = reader.ReadDictionaryLength();
            
            reader.ReadString(Encoding.UTF8);
            var name = reader.ReadString(Encoding.UTF8);
            
            reader.ReadString(Encoding.UTF8);
            var type = reader.ReadString(Encoding.UTF8);
            
            var isNullable = false;
            switch (length)
            {
                case FIELD_LENGTH:
                    break;
                
                case NULLABLE_FIELD_LENGTH:
                    reader.ReadString(Encoding.UTF8);
                    isNullable = reader.ReadBool();
                    break;
                
                default:
                    throw new ResponseParseException($"FieldMetaInfo fields length is invalid: {length}");
            }
            
            var fieldInfo = new FieldMetaInfo(type, name, isNullable);
            return fieldInfo;
        }
    }
}