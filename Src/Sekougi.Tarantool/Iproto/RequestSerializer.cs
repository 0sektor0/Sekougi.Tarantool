using System;
using System.IO;
using Sekougi.MessagePack;



namespace Sekougi.Tarantool.Iproto
{
    public class RequestSerializer : IDisposable
    {
        private const int IPROTO_REQUEST_TYPE_KEY = 0x00;
        private const int IPROTO_SCHEMA_VERSION = 0x05;
        private const int IPROTO_SYNC_KEY = 0x01;
        private const int LENGTH_OFFSET = 5;
        
        private IMessagePackBuffer _buffer;
        private MessagePackWriter _writer;
        private int _syncId; // temporary solution

        public RequestSerializer()
        {
            _buffer = new MessagePackStreamBuffer();
            _writer = new MessagePackWriter(_buffer);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }

        public ReadOnlySpan<byte> SerializeRequest(IRequest request)
        {
            _buffer.Seek(LENGTH_OFFSET, SeekOrigin.Begin);
            _writer.WriteDictionaryHeader(2);
            
            _writer.Write(IPROTO_REQUEST_TYPE_KEY);
            _writer.Write((int) request.Code);
            
            _writer.Write(IPROTO_SYNC_KEY);
            _writer.Write(_syncId++);
            
            request.Serialize(_writer);

            var reuestLength = _buffer.Length - LENGTH_OFFSET;
            _buffer.Seek(0, SeekOrigin.Begin);
            _writer.Write((uint)reuestLength, false);

            return _buffer.GetPart(0, _buffer.Length);
        }
    }
}