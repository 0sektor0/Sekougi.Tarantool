using System;
using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto.Requests
{
    public class NopRequest : RequestBase
    {
        public override RequestCode Code => RequestCode.Nop;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}