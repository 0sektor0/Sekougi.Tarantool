using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto
{
    public interface IRequest
    {
        RequestCode Code { get; }
        void Serialize(MessagePackWriter writer);
    }
}