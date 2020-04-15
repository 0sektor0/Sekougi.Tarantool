using Sekougi.Tarantool.Iproto.Requests;
using System.Collections.Generic;
using Sekougi.Tarantool.Iproto;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Model;
using System;


namespace Sekougi.Tarantool.ConsoleTest
{
    //Sandbox for some checks
    class Program
    {
        private static int _port = 3301;
        private static string _host = "127.0.0.1";
        private static string _login = "user_test";
        private static string _password = "user_test";
        
        
        public static void Main()
        {
            PlayWithConnection();
        }

        private static void PlayWithConnection()
        {
            using var connection = new Connection(_host, _port);
            connection.Connect(_login, _password);

            var spaceSelectRequest = new SelectRequest((uint) SystemSpaceE.Vspace, 0, IteratorE.All, 0U);
            var spaceSelectResponse = connection.SendRequest<Dictionary<int, Space[]>>(spaceSelectRequest);

            var indexSelectRequest = new SelectRequest((uint) SystemSpaceE.Vindex, 0, IteratorE.All, 0U) {SyncId = 2};
            var indexSelectResponse = connection.SendRequest<Dictionary<int, ValueTuple<uint, uint, string, string, Dictionary<string, bool>, ValueTuple<int, string>[]>>>(indexSelectRequest);
        }
    }
}