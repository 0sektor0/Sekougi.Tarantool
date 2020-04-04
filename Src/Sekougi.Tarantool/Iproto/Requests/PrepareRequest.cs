using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto.Requests
{
    public class PrepareRequest : RequestBase
    {
        public override RequestCode Code => RequestCode.Prepare;

        protected override void SerializeBody(MessagePackWriter writer)
        {

        }
    }
}