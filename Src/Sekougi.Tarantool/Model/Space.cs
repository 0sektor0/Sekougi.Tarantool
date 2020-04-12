using Sekougi.MessagePack.Serializers;
using Sekougi.Tarantool.Model.Serializers;



namespace Sekougi.Tarantool.Model
{
    [MessagePackSerialized(typeof(SpaceSerializer))]
    public class Space
    {
        public uint Id { get; }
        public uint OwnerId { get; }
        public string Name { get; }
        public string Engine { get; }
        public int FieldsCount { get; }
        public FlagsInfo Flags { get; }
        public FieldMetaInfo[] Fields { get; }

        
        public Space(uint id, uint ownerId, string name, string engine, int fieldsCount, 
            FlagsInfo flags, FieldMetaInfo[] fields)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Engine = engine;
            FieldsCount = fieldsCount;
            Flags = flags;
            Fields = fields;
        }
    }
}