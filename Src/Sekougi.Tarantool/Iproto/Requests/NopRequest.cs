using System;
using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class NopRequest : RequestBase
    {
        public override CommandE Code => CommandE.Nop;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}