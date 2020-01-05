using Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        private int num = 1;

        public async Task AddCountAsync(string taskName)
        {
            Console.WriteLine(taskName + "----" + num);
            num++;
            await Task.CompletedTask;
        }
    }
}
