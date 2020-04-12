using Sekougi.MessagePack.Serializers;
using System.Text;
using System;
using Sekougi.MessagePack;
using System.Collections.Generic;



namespace Sekougi.Tarantool.Model.Serializers
{
    public class FlagsInfoSerializer : MessagePackSerializer<FlagsInfo>
    {
        public override void Serialize(FlagsInfo value, MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }

        public override FlagsInfo Deserialize(MessagePackReader reader)
        {
            var flagsCount = reader.ReadDictionaryLength();
            var flags = new Dictionary<string, bool>(flagsCount);

            for (var i = 0; i < flagsCount; i++)
            {
                DeserializeFlag(reader, flags);
            }

            var flagsInfo = new FlagsInfo(flags);
            return flagsInfo;
        }

        private void DeserializeFlag(MessagePackReader reader, Dictionary<string, bool> destination)
        {
            var name = reader.ReadString(Encoding.UTF8);
            var value = name == "temporary" ? reader.ReadBool() : reader.ReadInt() > 0;
            
            destination.Add(name, value);
        }
    }
}