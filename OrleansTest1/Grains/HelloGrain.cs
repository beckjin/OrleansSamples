using Interfaces;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private string text = "Hello World!";

        public Task<string> SayHello(string greeting)
        {
            System.Threading.Thread.Sleep(3000);
            var oldText = text;
            text = greeting;
            Console.WriteLine(oldText + DateTime.Now);
            return Task.FromResult(oldText);
        }
    }
}
