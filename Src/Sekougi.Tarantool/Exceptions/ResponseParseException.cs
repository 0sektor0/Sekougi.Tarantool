using System;



namespace Sekougi.Tarantool.Exceptions
{
    public class ResponseParseException : Exception
    {
        public ResponseParseException(string message) : base(message)
        {

        }
    }
}