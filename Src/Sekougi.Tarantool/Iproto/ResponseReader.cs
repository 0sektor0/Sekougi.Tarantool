using Sekougi.MessagePack;
using System;
using System.IO;
using Sekougi.MessagePack.Buffers;



namespace Sekougi.Tarantool.Iproto
{
    public class ResponseReader : IDisposable
    {
        private MessagePackStreamBuffer _buffer;
        private MessagePackReader _reader;
        
        
        public ResponseReader(Stream stream)
        {
            _buffer = new MessagePackStreamBuffer(stream);
            _reader = new MessagePackReader(_buffer);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }

        public void Read()
        {
            var length = _reader.ReadUint();
            var header = new Header();
            header.Deserialize(_reader);

            var bodyMapLength = _reader.ReadDictionaryLength();
        }
    }
}