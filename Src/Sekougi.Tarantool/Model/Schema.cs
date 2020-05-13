using System.Collections.ObjectModel;
using Sekougi.Tarantool.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Sekougi.Tarantool.Iproto;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Iproto.Requests;
using SpaceData = System.ValueTuple<uint, uint, string, string, int, Sekougi.Tarantool.Model.FlagsInfo, Sekougi.Tarantool.Model.FieldMetaInfo[]>;
using IndexData = System.ValueTuple<uint, uint, string, string, System.Collections.Generic.Dictionary<string, bool>, System.ValueTuple<int, string>[]>;



namespace Sekougi.Tarantool.Model
{
    public class Schema
    {
        private readonly Connection _connection;
        private Dictionary<uint, Space> _spacesById;
        private Dictionary<string, Space> _spacesByName;

        public DateTime LastReloadTime { get; private set; }
        public IReadOnlyCollection<Space> Spaces { get; private set; }

        public Space this[uint id]
        {
            get
            {
                if (!_spacesById.TryGetValue(id, out var space))
                    throw new SpaceNotFoundException(id);
                
                return space;
            }
        }
        
        public Space this[string name]
        {
            get
            {
                if (!_spacesByName.TryGetValue(name, out var space))
                    throw new SpaceNotFoundException(name);
                
                return space;
            }
        }
        

        public Schema(Connection connection)
        {
            _connection = connection;
        }

        public bool ContainsSpace(uint id)
        {
            return _spacesById.ContainsKey(id);
        }

        public bool ContainsSpace(string name)
        {
            return _spacesByName.ContainsKey(name);
        }
        
        public void Reload()
        {
            var key = new ValueTuple<uint>(0u);
            var spaceSelectRequest = new SelectRequest<ValueTuple<uint>>((uint) SystemSpaceE.Vspace, 0, IteratorE.All, key);
            var spacesData = _connection.SendMultipleDataRequest<SpaceData>(spaceSelectRequest);
            
            var indexSelectRequest = new SelectRequest<ValueTuple<uint>>((uint) SystemSpaceE.Vindex, 0, IteratorE.All, key);
            var indexesData = _connection.SendMultipleDataRequest<IndexData>(indexSelectRequest);
            
            Initialize(spacesData, indexesData);
        }
        
        public async Task ReloadAsync()
        {
            var key = new ValueTuple<uint>(0u);
            var spaceSelectRequest = new SelectRequest<ValueTuple<uint>>((uint) SystemSpaceE.Vspace, 0, IteratorE.All, key);
            var spacesData = await _connection.SendMultipleDataRequestAsync<SpaceData>(spaceSelectRequest);
            
            var indexSelectRequest = new SelectRequest<ValueTuple<uint>>((uint) SystemSpaceE.Vindex, 0, IteratorE.All, key);
            var indexesData = await _connection.SendMultipleDataRequestAsync<IndexData>(indexSelectRequest);
            
            Initialize(spacesData, indexesData);
        }

        private void Initialize(SpaceData[] spacesData, IndexData[] indexesData)
        {
            var indexesBySpaceId = new Dictionary<uint, List<Index>>();
            foreach (var indexData in indexesData)
            {
                var index = new Index(indexData, _connection);

                if (!indexesBySpaceId.TryGetValue(index.SpaceId, out var spaceIndexes))
                {
                    spaceIndexes = new List<Index>();
                    indexesBySpaceId.Add(index.SpaceId, spaceIndexes);
                }
                
                spaceIndexes.Add(index);
            }
            
            var spaces = new List<Space>();
            foreach (var spaceData in spacesData)
            {
                var space = new Space(spaceData, _connection);
                spaces.Add(space);

                var spaceIndexes = indexesBySpaceId.GetValueOrDefault(space.Id);
                space.SetIndexes(spaceIndexes);
            }

            Spaces = new ReadOnlyCollection<Space>(spaces);
            _spacesById = Spaces.ToDictionary(space => space.Id);
            _spacesByName = Spaces.ToDictionary(space => space.Name);
            
            LastReloadTime = DateTime.Now;
        }
    }
}