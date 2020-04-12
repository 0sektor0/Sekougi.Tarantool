using Sekougi.MessagePack.Serializers;
using Sekougi.Tarantool.Model.Serializers;



namespace Sekougi.Tarantool.Model
{
    [MessagePackSerialized(typeof(FieldMetaInfoSerializer))]
    public readonly struct FieldMetaInfo
    {
        public readonly string Type;
        public readonly string Name;
        public readonly bool IsNullable;


        public FieldMetaInfo(string type, string name, bool isNullable)
        {
            Type = type;
            Name = name;
            IsNullable = isNullable;
        }
    }
}