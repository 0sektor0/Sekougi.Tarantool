using Sekougi.MessagePack.Buffers;
using Sekougi.MessagePack;
using System.IO;
using System.Text;
using Sekougi.Tarantool.Exceptions;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Iproto.Responses;



namespace Sekougi.Tarantool.Iproto
{
    //TODO: create pool for responses
    public class ResponseReader
    {
        private readonly MessagePackReader _reader;
        
        
        public ResponseReader(Stream stream)
        {
            var buffer = new MessagePackStreamBuffer(stream);
            _reader = new MessagePackReader(buffer);
        }

        public Response Read()
        {
            var syncId = ReadId();
            var response = new Response();
            response.Initialize(syncId, _reader);

            return response;
        }
        
        public DataResponse<T> Read<T>()
        {
            var syncId = ReadId();
            var response = new DataResponse<T>();
            response.Initialize(syncId, _reader);
            
            return response;
        }

        private int ReadId()
        {
            var length = _reader.ReadUint();
            var header = new Header();
            header.Deserialize(_reader);
            
            var isError = header.Code >= CommandE.ErrorMin && header.Code < CommandE.ErrorMax;
            if (isError)
            {
                var errorMessage = ReadErrorMessage();
                _reader.ReadRawBytes((int) length);
                
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