using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.MessagePack;
using System;



namespace Sekougi.Tarantool.Iproto.UpdateOperations
{
    public readonly struct SpliceOperation : IUpdateOperation
    {
        private readonly int _fieldNumber;
        private readonly int _position;
        private readonly int _offset;
        private readonly string _value;

        
        public SpliceOperation(int fieldNumber, int position, int offset, string value)
        {
            _fieldNumber = fieldNumber;
            _position = position;
            _offset = offset;
            _value = value;
        }


        public void Serialize(MessagePackWriter writer)
        {
            var tuple = (UpdateOperatorConverter.SPLICE_CODE, _fieldNumber, _position, _offset, _value);
            MessagePackSerializersRepository.Get<ValueTuple<string, int, int, int, string>>().Serialize(tuple, writer);
        }
    }
}