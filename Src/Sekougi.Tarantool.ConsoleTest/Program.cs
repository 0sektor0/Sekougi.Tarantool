using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Sekougi.MessagePack;
using Sekougi.MessagePack.Buffers;
using Sekougi.Tarantool.Iproto;
using Sekougi.Tarantool.Iproto.Requests;


namespace Sekougi.Tarantool.ConsoleTest
{
    //Sandbox for some checks
    class Program
    {
        static int port = 3301;
        static string host = "127.0.0.1";
        
        
        public static void Main()
        {
            PlayWithConnection();
        }

        private static void PlayWithConnection()
        {
            using var tcpClient = new TcpClient(host, port);
            var stream = tcpClient.GetStream();
            var buffer = new byte[512];
            
            using var requestWriter = new RequestWriter(stream);
            using var responseReader = new ResponseReader(stream);

            stream.Read(buffer);
            var base64Salt = new ReadOnlySpan<byte>(buffer, 64, 44);
            var authRequest = new AuthRequest("user_test", "user_test", base64Salt);
            
            authRequest.SyncId = 1;
            requestWriter.Write(authRequest);
            responseReader.Read();
            
            authRequest.SyncId = 2;
            requestWriter.Write(authRequest);
            responseReader.Read();
        }

        private static void SniffBytes(Stream stream)
        {
            var reader = new MessagePackReader(new MessagePackStreamBuffer(stream));
            var length = reader.ReadUint();
            var bytes = new byte[length];
            stream.Read(bytes);

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                stringBuilder.Append($"{bytes[i]}, ");
            }
            var result = $"[{stringBuilder}]";
        }
    }
}