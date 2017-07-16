using System.Threading.Tasks;
using Orleans;
using GrainInterfaces1;
using System;

namespace Grains1
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class Grain1 : Grain, IGrain1
    {
        public Task<string> SayHello()
        {
            return Task.FromResult("Hello World!");
        }
    }
}
