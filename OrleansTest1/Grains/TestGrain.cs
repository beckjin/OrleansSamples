using Interfaces;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class TestGrain : Orleans.Grain, ITest
    {
        private int num = 0;

        public Task AddCount()
        {
            num++;
            Console.WriteLine(num);
            return Task.CompletedTask;
        }
    }
}
