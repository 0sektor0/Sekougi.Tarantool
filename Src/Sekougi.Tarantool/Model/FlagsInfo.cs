using System.Collections.Generic;
using Sekougi.MessagePack.Serializers;
using Sekougi.Tarantool.Model.Serializers;



namespace Sekougi.Tarantool.Model
{
    [MessagePackSerialized(typeof(FlagsInfoSerializer))]
    public class FlagsInfo
    {
        private readonly Dictionary<string, bool> _flags;

        
        public FlagsInfo(Dictionary<string, bool> flags)
        {
            _flags = flags;
        }

        public bool IsFlagActive(string name)
        {
            return _flags.GetValueOrDefault(name);
        }
    }
}