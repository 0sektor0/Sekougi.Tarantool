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
        
        private IMessagePackBuffer _buffer;
        private MessagePackWriter _writer;
        private Stream _destination;

        
        public RequestWriter(Stream destination)
        {
            _buffer = new MessagePackMemoryStreamBuffer();
            _writer = new MessagePackWriter(_buffer);
            _destination = destination;
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }

        public void Write(RequestBase request)
        {
            var serializedRequest = SerializeRequest(request);
            _destination.Write(serializedRequest);
        }

        private ReadOnlySpan<byte> SerializeRequest(RequestBase request)
        {
            _buffer.Seek(LENGTH_OFFSET, SeekOrigin.Begin);
            request.Serialize(_writer);

            var requestLength = _buffer.Length - LENGTH_OFFSET;
            _buffer.Seek(0, SeekOrigin.Begin);
            _writer.Write((uint)requestLength, false);

            return _buffer.GetPart(0, _buffer.Length);
        }
    }
}