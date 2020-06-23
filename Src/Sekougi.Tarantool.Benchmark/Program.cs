using BenchmarkDotNet.Running;



namespace Sekougi.Tarantool.Benchmark
{
    class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}