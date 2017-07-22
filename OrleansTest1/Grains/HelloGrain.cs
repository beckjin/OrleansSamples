using Interfaces;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        public Task<string> SayHello(string greeting)
        {
            System.Threading.Thread.Sleep(3000);
            var str = greeting + DateTime.Now;
            Console.WriteLine(str);
            return Task.FromResult(str);
        }
    }
}
