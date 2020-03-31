using System;
using System.Net;
using System.Net.Sockets;
using Sekougi.MessagePack;
using Sekougi.Tarantool.Iproto;



namespace Sekougi.Tarantool.ConsoleTest
{
    //Sandbox for some checks
    class Program
    {
        static int port = 3301;
        static string address = "127.0.0.1";
        
        
        public static void Main()
        {
            ConnectionTest();
        }

        private static void ConnectionTest()
        {
            
            var ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            var buffer = new byte[512];
            
            socket.Connect(ipPoint);
            socket.Receive(buffer);

            var base64Salt = new ReadOnlySpan<byte>(buffer, 64, 44);
            var authRequest = new AuthRequest("user_test", "user_test", base64Salt);
            
            using var requestSerializer = new RequestSerializer();
            var requestBytes = requestSerializer.SerializeRequest(authRequest);

            socket.Send(requestBytes);
            socket.Receive(buffer);
            
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            
            DecodeResponse(buffer);
        }

        private static void DecodeResponse(byte[] data)
        {
            using var buffer = new MessagePackStreamBuffer();
            buffer.Write(data);
            buffer.Drop();
            
            var reader = new MessagePackReader(buffer);
        }
    }
}