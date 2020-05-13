using System;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Iproto.UpdateOperations;
using Sekougi.Tarantool.Model;
using Redocrd = System.ValueTuple<uint, string, uint>;


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
            
            var schema = new Schema(connection);
            schema.ReloadAsync().GetAwaiter().GetResult();

            var testSpace = schema["tester"];

            var updateOperation = new UpdateOperation<string>(UpdateOperatorE.Assignment, 1, "updated");
            var key = new ValueTuple<int>(16);
            var data = testSpace.Update<ValueTuple<int>, Redocrd>(0, key, updateOperation);

            var index = testSpace["primary"];
            data = index.Select<ValueTuple<uint>, Redocrd>(UInt32.MaxValue, 0, IteratorE.All, new ValueTuple<uint>(0));
        }
    }
}