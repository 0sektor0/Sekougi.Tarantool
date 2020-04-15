using Sekougi.Tarantool.Iproto.Requests;
using Sekougi.Tarantool.Iproto;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using Sekougi.Tarantool.Iproto.Responses;



namespace Sekougi.Tarantool
{
    public class Connection : IDisposable
    {
        private readonly int _port;
        private readonly string _host;
        
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private RequestWriter _requestWriter;
        private ResponseReader _responseReader;
        private RequestCounter _requestCounter;
        private bool _isDisposed;
        

        
        public Connection(string host, int port)
        {
            _isDisposed = true;
            _port = port;
            _host = host;
            _requestCounter = new RequestCounter();
        }
        
        public void Dispose()
        {
            _tcpClient.Dispose();
            _tcpClient = null;
            _networkStream = null;
            
            _requestWriter.Dispose();
            _requestWriter = null;
            _responseReader = null;
            
            _requestCounter.Drop();
        }

        public void Connect(string user, string password)
        {
            if (!_isDisposed)
                Dispose();
            
            _tcpClient = new TcpClient(_host, _port);
            _networkStream = _tcpClient.GetStream();
            _requestWriter = new RequestWriter(_networkStream);
            _responseReader = new ResponseReader(_networkStream);
            
            Authorize(user, password);
        }

        public void SendRequest(RequestBase request)
        {
            request.SyncId = _requestCounter.GetNextId();
            _requestWriter.Write(request);
            _responseReader.Read();
        }

        // TODO: that's very bad, but i have no time to do better
        public Task SendRequestAsync(RequestBase request)
        {
            return Task.Run(() => SendRequest(request));
        }

        public DataResponse<T> SendRequest<T>(RequestBase request)
        {
            request.SyncId = _requestCounter.GetNextId();
            _requestWriter.Write(request);
            var response = _responseReader.Read<T>();

            return response;
        }

        // TODO: that's very bad, but i have no time to do better
        public Task<DataResponse<T>> SendRequestAsync<T>(RequestBase request)
        {
            return Task.Run(() => SendRequest<T>(request));
        }

        private void Authorize(string user, string password)
        {
            Span<byte> buffer = stackalloc byte[512];
            _networkStream.Read(buffer);

            var base64Salt = buffer.Slice(64, 44);

            var authRequest = new AuthRequest(user, password, base64Salt);
            SendRequest(authRequest);
        }
    }
}