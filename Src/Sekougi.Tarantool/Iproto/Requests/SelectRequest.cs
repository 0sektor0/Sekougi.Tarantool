using System;
using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto.Requests
{
    public class SelectRequest : RequestBase
    {
        public override RequestCode Code => RequestCode.Select;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}