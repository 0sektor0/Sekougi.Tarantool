using System;
using System.IO;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Iproto.UpdateOperations;
using Sekougi.Tarantool.Model;



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
            var data = testSpace.Update<ValueTuple<int>, ValueTuple<uint, string, uint>>(0, key, updateOperation);

            var index = testSpace["primary"];
            var allData = index.Select<ValueTuple<uint, string, uint>, uint>(UInt32.MaxValue, 0, IteratorE.All);
            /*var dataToInsert = (16u, "0", 1997u);
            testSpace.InsertAsync(dataToInsert);
            
            var dataToReplace = (16u, "1", 2000u);
            var replaceResult = testSpace.ReplaceAsync(dataToReplace).GetAwaiter().GetResult();*/
        }
    }
}