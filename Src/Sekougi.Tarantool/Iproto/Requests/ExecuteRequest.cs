using System;
using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto.Requests
{
    public class ExecuteRequest : RequestBase
    {
        public override RequestCode Code => RequestCode.Execute;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}