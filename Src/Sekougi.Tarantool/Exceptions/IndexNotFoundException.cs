using System;



namespace Sekougi.Tarantool.Exceptions
{
    public class IndexNotFoundException : Exception
    {
        public IndexNotFoundException(string spaceName) : base($"index with name: {spaceName} not found")
        {
            
        }
        
        public IndexNotFoundException(uint spaceId) : base($"index with id: {spaceId} not found")
        {
            
        }
    }
}