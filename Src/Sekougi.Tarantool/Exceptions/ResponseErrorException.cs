using System;



namespace Sekougi.Tarantool.Exceptions
{
    public class ResponseErrorException : Exception
    {
        public ResponseErrorException(int syncId, string error) : base($"request with id: {syncId} failed with error: {error}")
        {
            
        }
    }
}