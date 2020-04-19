using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.MessagePack;
using System;



namespace Sekougi.Tarantool.Iproto.UpdateOperations
{
    public readonly struct DeletionOperation : IUpdateOperation
    {
        private readonly int _fieldNumber;

        
        public DeletionOperation(int fieldNumber)
        {
            _fieldNumber = fieldNumber;
        }

        public void Serialize(MessagePackWriter writer)
        {
            var tuple = (UpdateOperatorConverter.DELETION_CODE, _fieldNumber);
            MessagePackSerializersRepository.Get<ValueTuple<string, int>>().Serialize(tuple, writer);
        }
    }
}