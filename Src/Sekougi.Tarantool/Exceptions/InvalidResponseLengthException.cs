using System;



namespace Sekougi.Tarantool.Exceptions
{
    public class InvalidResponseLengthException : Exception
    {
        public InvalidResponseLengthException(int id, int expected, int get) 
            : base($"invalid response data length for id: {id} expected objects: {expected} get: {get}")
        {
            
        }
        
        public InvalidResponseLengthException(int id, int get) 
            : base($"invalid response data length for id: {id} expected array of objects but get: {get}")
        {
            
        }
    }
}