using System.Runtime.CompilerServices;
using Sekougi.Tarantool.Iproto.Enums;



namespace Sekougi.Tarantool.Iproto.Requests
{
    public class ReplaceRequest<T> : InsertRequest<T> where T : ITuple
    {
        public override CommandE Code => CommandE.Replace;
        
        
        public ReplaceRequest(uint spaceId,  T dataToInsert) : base(spaceId, dataToInsert)
        {
            
        }
    }
}