using Sekougi.MessagePack.Buffers;
using Sekougi.MessagePack;
using System.IO;
using System.Text;
using Sekougi.Tarantool.Exceptions;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto
{
    //TODO: create pool for responses
    public class ResponseReader
    {
        private readonly object _lock = new object();
        private readonly MessagePackReader _reader;
        
        
        public ResponseReader(Stream stream)
        {
            var buffer = new MessagePackStreamBuffer(stream);
            _reader = new MessagePackReader(buffer);
        }

        public void ReadEmptyData()
        {
            lock (_lock)
            {
                ReadEmptyDataInternal();
            }
        }
        
        public T ReadSingleData<T>()
        {
            lock (_lock)
            {
                return ReadSingleDataInternal<T>();
            }
        }
        
        public T[] ReadMultipleData<T>()
        {
            lock (_lock)
            {
                return ReadMultipleDataInternal<T>();
            }
        }

        private void ReadEmptyDataInternal()
        {
            var (syncId, objectsCount) = ReadResponseInfo();
            if (objectsCount != 0)
                throw new InvalidResponseLengthException(syncId, 0, objectsCount);
        }
        
        private T ReadSingleDataInternal<T>()
        {
            var (syncId, objectsCount) = ReadResponseInfo();
            
            var dataLength = objectsCount;
            if (dataLength != 1)
                throw new InvalidResponseLengthException(syncId, 1, objectsCount);

            return MessagePackSerializersRepository.Get<T>().Deserialize(_reader);
        }

        private T[] ReadMultipleDataInternal<T>()
        {
            var syncId = ReadId();
            
            var dictionaryLength = _reader.ReadDictionaryLength();
            if (dictionaryLength == 0)
                return new T[0];

            var key = _reader.ReadInt();
            var dataArray = MessagePackSerializersRepository.Get<T[]>().Deserialize(_reader);

            return dataArray;
        }

        private (int syncId, int objectsCount) ReadResponseInfo()
        {
            var syncId = ReadId();
            
            var dictionaryLength = _reader.ReadDictionaryLength();
            if (dictionaryLength == 0)
                return (syncId, 0);

            var key = _reader.ReadInt();
            return (syncId, _reader.ReadArrayLength());
        }

        private int ReadId()
        {
            _reader.ReadUint();
            var header = new Header();
            header.Deserialize(_reader);
            
            // TODO: add exception for each error
            var isError = header.Code >= CommandE.ErrorMin && header.Code < CommandE.ErrorMax;
            if (isError)
            {
                var errorMessage = ReadErrorMessage();
                throw new ResponseErrorException(header.SyncId, errorMessage);
            }

            return header.SyncId;
        }

        private string ReadErrorMessage()
        {
            _reader.ReadDictionaryLength();
            _reader.ReadUint();
            var message = _reader.ReadString(Encoding.UTF8);

            return message;
        }
    }
}