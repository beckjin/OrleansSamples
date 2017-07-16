using HelloWorld.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private string text = "Hello World!";

        public Task<string> SayHello(string greeting)
        {
            var oldText = text;
            text = greeting;
            return Task.FromResult(oldText);
        }
    }
}
