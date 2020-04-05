using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto.Requests
{
    public class PrepareRequest : RequestBase
    {
        public override CommandE Code => CommandE.Prepare;

        protected override void SerializeBody(MessagePackWriter writer)
        {

        }
    }
}