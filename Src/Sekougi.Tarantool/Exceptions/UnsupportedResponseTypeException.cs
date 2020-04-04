using System;
using Sekougi.Tarantool.Iproto;



namespace Sekougi.Tarantool.Exceptions
{
    public class UnsupportedResponseTypeException : Exception
    {
        public UnsupportedResponseTypeException(RequestCode code) : base($"Unsupported response type: {code}")
        {
            
        }
    }
}