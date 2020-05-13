using Sekougi.Tarantool.Iproto.UpdateOperations;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sekougi.Tarantool.Exceptions;
using Sekougi.Tarantool.Iproto.Enums;
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

        public TData[] Select<TData, TKey>(uint indexId, uint limit, uint offset, IteratorE iterator, params TKey[] key)
        {
            return _indexesById[indexId].Select<TData, TKey>(limit, offset, iterator, key);
        }

        public Task<TData[]> SelectAsync<TData, TKey>(uint indexId, uint limit, uint offset, IteratorE iterator, params TKey[] key)
        {
            return _indexesById[indexId].SelectAsync<TData, TKey>(limit, offset, iterator, key);
        }

        public TData[] Select<TData, TKey>(string indexName, uint limit, uint offset, IteratorE iterator, params TKey[] key)
        {
            return _indexesByName[indexName].Select<TData, TKey>(limit, offset, iterator, key);
        }

        public Task<TData[]> SelectAsync<TData, TKey>(string indexName, uint limit, uint offset, IteratorE iterator, params TKey[] key)
        {
            return _indexesByName[indexName].SelectAsync<TData, TKey>(limit, offset, iterator, key);
        }
        
        public T Insert<T>(T dataToInsert) where T : ITuple
        {
            var insertRequest = new InsertRequest<T>(Id, dataToInsert);
            return _connection.SendSingleDataRequest<T>(insertRequest);
        }

        public async Task<T> InsertAsync<T>(T dataToInsert) where T : ITuple
        {
            var insertRequest = new InsertRequest<T>(Id, dataToInsert);
            return await _connection.SendSingleDataRequestAsync<T>(insertRequest);
        }

        public T Replace<T>(T dataToInsert) where T : ITuple
        {
            var replaceRequest = new ReplaceRequest<T>(Id, dataToInsert);
            return _connection.SendSingleDataRequest<T>(replaceRequest);
        }

        public async Task<T> ReplaceAsync<T>(T dataToInsert) where T : ITuple
        {
            var replaceRequest = new ReplaceRequest<T>(Id, dataToInsert);
            return await _connection.SendSingleDataRequestAsync<T>(replaceRequest);
        }

        public TData[] Update<TKey, TData>(uint indexId, TKey key, params IUpdateOperation[] updateOperations) where TKey : ITuple
        {
            var updateRequest = new UpdateRequest<TKey>(Id, indexId, key, updateOperations);
            return _connection.SendMultipleDataRequest<TData>(updateRequest);
        }

        public async Task<TData[]> UpdateAsync<TKey, TData>(uint indexId, TKey key, params IUpdateOperation[] updateOperations) where TKey : ITuple
        {
            var updateRequest = new UpdateRequest<TKey>(Id, indexId, key, updateOperations);
            return await _connection.SendMultipleDataRequestAsync<TData>(updateRequest);
        }
    }
}