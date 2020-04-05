using System;
using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class EvalRequest : RequestBase
    {
        public override CommandE Code => CommandE.Eval;

        protected override void SerializeBody(MessagePackWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}