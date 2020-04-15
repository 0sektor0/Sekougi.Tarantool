using Sekougi.Tarantool.Model.Enums;
using System.Collections.Generic;
using System;



namespace Sekougi.Tarantool.Model
{
    public class Index
    {
        public uint SpaceId { get; }
        public uint IdInSpace { get; }
        public string Name { get; }
        public IndexTypeE Type { get; }
        public Dictionary<string, bool> Options { get; }
        public ValueTuple<int, string> KeyTypes { get; }
    }
}