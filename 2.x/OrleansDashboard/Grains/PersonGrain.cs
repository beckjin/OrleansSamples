using Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class PersonGrain : Grain, IPersonGrain
    {
        public async Task SayHelloAsync()
        {
            string primaryKey = this.GetPrimaryKeyString();

            Console.WriteLine($"{primaryKey} said hello!");

            await Task.CompletedTask;
        }
    }
}
