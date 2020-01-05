using Interfaces;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class TestGrain : Orleans.Grain, ITestGrain
    {
        private int num = 1;

        public Task AddCount(string taskName)
        {
            Console.WriteLine(taskName + "----" + num);
            num++;
            return Task.CompletedTask;
        }
    }
}
