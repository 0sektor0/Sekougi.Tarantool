using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.MessagePack;
using System;



namespace Sekougi.Tarantool.Iproto.UpdateOperations
{
    public readonly struct UpdateOperation<T> : IUpdateOperation
    {
        private readonly UpdateOperatorE _updateOperator;
        private readonly int _fieldNumber;
        private readonly T _value;

        
        public UpdateOperation(UpdateOperatorE updateOperator, int fieldNumber, T value)
        {
            _updateOperator = updateOperator;
            _fieldNumber = fieldNumber;
            _value = value;
        }

        public void Serialize(MessagePackWriter writer)
        {
            var operatorCode = UpdateOperatorConverter.ToString(_updateOperator);
            var tuple = (operatorCode, _fieldNumber, value: _value);
            MessagePackSerializersRepository.Get<ValueTuple<string, int, T>>().Serialize(tuple, writer);
        }
    }
}