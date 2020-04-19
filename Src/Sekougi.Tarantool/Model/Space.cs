using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Sekougi.Tarantool.Exceptions;
using Sekougi.Tarantool.Iproto.Requests;
using SpaceData = System.ValueTuple<uint, uint, string, string, int, Sekougi.Tarantool.Model.FlagsInfo, Sekougi.Tarantool.Model.FieldMetaInfo[]>;



namespace Sekougi.Tarantool.Model
{
    public class Space
    {
        private Connection _connection;
        private Dictionary<uint, Index> _indexesById;
        private Dictionary<string, Index> _indexesByName;
        
        public uint Id { get; }
        public uint OwnerId { get; }
        public string Name { get; }
        public string Engine { get; }
        public int FieldsCount { get; }
        public FlagsInfo Flags { get; }
        public IReadOnlyCollection<FieldMetaInfo> Fields { get; }    
        public IReadOnlyCollection<Index> Indexes { get; private set; }

        public Index this[string name]
        {
            get
            {
                if (!_indexesByName.TryGetValue(name, out var index))
                    throw new IndexNotFoundException(name);

                return index;
            }
        }

        public Index this[uint id]
        {
            get
            {
                if (!_indexesById.TryGetValue(id, out var index))
                    throw new IndexNotFoundException(id);

                return index;
            }
        }

        
        public Space(SpaceData spaceData, Connection connection)
        {
            _connection = connection;

            Id = spaceData.Item1;
            OwnerId = spaceData.Item2;
            Name = spaceData.Item3;
            Engine = spaceData.Item4;
            FieldsCount = spaceData.Item5;
            Flags = spaceData.Item6;
            
            Fields = new ReadOnlyCollection<FieldMetaInfo>(spaceData.Item7);
        }

        internal void SetIndexes(IList<Index> indexes)
        {
            if (indexes == null)
                indexes = new List<Index>();
            
            foreach (var index in indexes)
            {
                index.SetSpace(this);
            }
            
            Indexes = new ReadOnlyCollection<Index>(indexes);
            _indexesById = Indexes.ToDictionary(index => index.Id);
            _indexesByName = Indexes.ToDictionary(index => index.Name);
        }

        public T Insert<T>(T dataToInsert) where T : ITuple
        {
            var insertRequest = new InsertRequest<T>(Id, dataToInsert);
            var result = _connection.SendRequest<Dictionary<int, T[]>>(insertRequest).Values.First().First();
            
            return result;
        }

        public async Task<T> InsertAsync<T>(T dataToInsert) where T : ITuple
        {
            var insertRequest = new InsertRequest<T>(Id, dataToInsert);
            var response = await _connection.SendRequestAsync<Dictionary<int, T[]>>(insertRequest);
            var result = response.Values.First().First();
            
            return result;
        }
    }
}