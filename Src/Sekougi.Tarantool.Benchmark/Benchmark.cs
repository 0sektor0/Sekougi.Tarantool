using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using Sekougi.Tarantool.Iproto.Enums;
using Sekougi.Tarantool.Iproto.Requests;
using Sekougi.Tarantool.Iproto.UpdateOperations;
using Schema = Sekougi.Tarantool.Model.Schema;
using Redocrd = System.ValueTuple<uint, string, uint>;


namespace Sekougi.Tarantool.Benchmark
{
    //[SimpleJob(RuntimeMoniker.Net472)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp30)]
    //[SimpleJob(RuntimeMoniker.CoreRt30)]
    //[SimpleJob(RuntimeMoniker.Mono)]
    [MemoryDiagnoser]
    //[RPlotExporter]
    public class Benchmark
    {
        private static int _port = 3301;
        private static string _host = "127.0.0.1";
        private static string _login = "user_test";
        private static string _password = "user_test";
        
        private Connection _connection;
        private UpdateOperation<string> _updateOperationSekougi;
        private Schema _schemaSekougi;
        private PingRequest _pingRequest;
        
        private Box _box;
        private SelectOptions _selectAll;
        private ProGaudi.Tarantool.Client.Model.UpdateOperations.UpdateOperation<string>[] _updateOperationsPro;
        private ISchema _schemaPro;
        
        
        
        [GlobalSetup]
        public void Setup()
        {
            _connection = new Connection(_host, _port);
            _connection.Connect(_login, _password);
            
            _schemaSekougi = new Schema(_connection);
            _schemaSekougi.ReloadAsync().GetAwaiter().GetResult();
            _pingRequest = new PingRequest();
            _updateOperationSekougi = new UpdateOperation<string>(UpdateOperatorE.Assignment, 1, "updated");

            _box = Box.Connect(_host, _port, _login, _password).GetAwaiter().GetResult();
            _updateOperationsPro = new []
            {
                new ProGaudi.Tarantool.Client.Model.UpdateOperations.UpdateOperation<string>("=", 1, "updated")
            };
            
            _schemaPro = _box.Schema;
            _selectAll = new SelectOptions
            {
                Iterator = Iterator.All
            };
        }


        [IterationSetup]
        public void IterationSetup()
        {
            
        }
        
        [IterationCleanup]
        public void IterationCleanup()
        {
            
        }
        
        
        /*[Benchmark]
        [Arguments(1)]
        [Arguments(100)]
        public void SerializeTupleCli(int count)
        {
            
        }*/
        
        //[Benchmark]
        public void SchemeReloadSekougi()
        {
            try
            {
                _schemaSekougi.ReloadAsync().GetAwaiter().GetResult();
            }
            catch
            {
                
            }
        }
        
        //[Benchmark]
        public void SchemeReloadPro()
        {
            try
            {
                _schemaPro.Reload().GetAwaiter().GetResult();
            }
            catch
            {
                
            }
        }

        //[Benchmark]
        public void SelectPro()
        {
            _schemaPro["tester"]["primary"]
                .Select<TarantoolTuple<uint>, TarantoolTuple<uint, string, uint>>(new TarantoolTuple<uint>(0), _selectAll)
                .GetAwaiter().GetResult();
        }

        //[Benchmark]
        public void SelectSekougi()
        {
            _schemaSekougi["tester"]["primary"]
                .SelectAsync<ValueTuple<uint>, Redocrd>(UInt32.MaxValue, 0, IteratorE.All, new ValueTuple<uint>(0))
                .GetAwaiter().GetResult();
        }

        //[Benchmark]
        public void UpdatePro()
        {
            _schemaPro["tester"]
                .Update<TarantoolTuple<int>,TarantoolTuple<uint, string, uint>>(new TarantoolTuple<int>(16), _updateOperationsPro)
                .GetAwaiter().GetResult();
        }

        //[Benchmark]
        public void UpdateSekougi()
        {
            _schemaSekougi["tester"]
                .UpdateAsync<ValueTuple<int>, Redocrd>(0, new ValueTuple<int>(16), _updateOperationSekougi)
                .GetAwaiter().GetResult();
        }

        [Benchmark]
        public void PingSekougi()
        {
            _connection.SendEmptyDataRequestAsync(_pingRequest).GetAwaiter().GetResult();
        }
    }
}