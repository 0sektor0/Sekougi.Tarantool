using Sekougi.MessagePack.Buffers;
using Sekougi.MessagePack;
using System.IO;
using Sekougi.Tarantool.Exceptions;
using Sekougi.Tarantool.Iproto.Responses;



namespace Sekougi.Tarantool.Iproto
{
    public class ResponseReader
    {
        private readonly ResponseFactory _responseFactory;
        private readonly MessagePackReader _reader;
        
        
        public ResponseReader(Stream stream)
        {
            var buffer = new MessagePackStreamBuffer(stream);
            _reader = new MessagePackReader(buffer);
            _responseFactory = new ResponseFactory();
        }

        public ResponseBase Read()
        {
            var length = _reader.ReadUint();
            var header = new Header();
            header.Deserialize(_reader);

            var response = _responseFactory.Create(header.Code);
            if (response != null)
            {
                response.Deserialize(_reader, header.SyncId);
                return response;
            }

            _reader.ReadRawBytes((int) length);
            throw new UnsupportedResponseTypeException(header.Code);
        }
    }
}