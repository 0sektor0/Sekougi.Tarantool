using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;



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