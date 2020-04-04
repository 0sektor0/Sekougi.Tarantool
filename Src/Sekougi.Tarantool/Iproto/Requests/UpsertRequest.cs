using System;
using Sekougi.MessagePack;

namespace Sekougi.Tarantool.Iproto.Requests
{
    public class UpsertRequest : RequestBase
    {
        public override RequestCode Code => RequestCode.Upsert;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}