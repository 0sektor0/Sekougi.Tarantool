using System;



namespace Sekougi.Tarantool.Exceptions
{
    public class SpaceNotFoundException : Exception
    {
        public SpaceNotFoundException(string spaceName) : base($"space with name: {spaceName} not found")
        {
            
        }
        
        public SpaceNotFoundException(uint spaceId) : base($"space with id: {spaceId} not found")
        {
            
        }
    }
}