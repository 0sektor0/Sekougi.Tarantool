using Sekougi.Tarantool.Iproto.Requests;
using System;



namespace Sekougi.Tarantool.Exceptions
{
    public class UnsupportedResponseTypeException : Exception
    {
        public UnsupportedResponseTypeException(RequestCode code) : base($"Unsupported response type: {code}")
        {
            
        }
    }
}