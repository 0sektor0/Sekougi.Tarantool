using Sekougi.Tarantool.Iproto.Requests;
using System.IO;
using System;
using Sekougi.MessagePack;
using Sekougi.MessagePack.Buffers;



namespace Sekougi.Tarantool.Iproto
{
    public class RequestWriter : IDisposable
    {
        private const int LENGTH_OFFSET = 5;
        
        private readonly MessagePackBuffer _serializationBuffer;
        private readonly MessagePackWriter _writer;
        private readonly Stream _destination;

        
        public RequestWriter(Stream destination)
        {
            _serializationBuffer = new MessagePackBuffer();
            _writer = new MessagePackWriter(_serializationBuffer);
            _destination = destination;
        }

        public void Dispose()
        {
            _serializationBuffer.Dispose();
        }

        public void Write(RequestBase request)
        {
            var serializedRequest = SerializeRequest(request);
            _destination.Write(serializedRequest);
        }

        private ReadOnlySpan<byte> SerializeRequest(RequestBase request)
        {
            _serializationBuffer.Clear();
            
            _serializationBuffer.Seek(LENGTH_OFFSET, SeekOrigin.Begin);
            request.Serialize(_writer);

            var requestLength = _serializationBuffer.Length - LENGTH_OFFSET;
            _serializationBuffer.Seek(0, SeekOrigin.Begin);
            _writer.Write((uint)requestLength, false);

            return _serializationBuffer.GetPart(0, _serializationBuffer.Length);
        }
    }
}