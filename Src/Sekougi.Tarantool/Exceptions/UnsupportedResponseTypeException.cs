using System;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Exceptions
{
    public class UnsupportedResponseTypeException : Exception
    {
        public UnsupportedResponseTypeException(CommandE code) : base($"Unsupported response type: {code}")
        {
            
        }
    }
}