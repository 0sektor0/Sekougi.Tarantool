using System.Threading;



namespace Sekougi.Tarantool
{
    public class RequestCounter
    {
        private int _lastId = -1;


        public void Drop()
        {
            _lastId = -1;
        }
        
        public int GetNextId()
        {
            return Interlocked.Increment(ref _lastId);
        }
    }
}