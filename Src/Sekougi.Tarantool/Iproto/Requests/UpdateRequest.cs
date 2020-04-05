using System;
using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class UpdateRequest : RequestBase
    {
        public override CommandE Code => CommandE.Update;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}