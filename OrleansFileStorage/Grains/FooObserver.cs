using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    class FooObserver : IFooGrainObserver
    {
        public void Foo(string message)
        {
            Console.WriteLine(message);
        }
    }
}
