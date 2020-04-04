using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using Sekougi.MessagePack;
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
    }
}