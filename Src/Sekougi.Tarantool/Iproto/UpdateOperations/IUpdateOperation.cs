using Sekougi.MessagePack;


namespace Sekougi.Tarantool.Iproto.UpdateOperations
{
    public interface IUpdateOperation
    {
        void Serialize(MessagePackWriter writer);
    }
}