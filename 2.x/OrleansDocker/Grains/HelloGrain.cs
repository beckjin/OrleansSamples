using Interfaces;
using Orleans;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloGrain : Grain, IHelloGrain
    {
        public async Task<string> SayHelloAsync(string greeting)
        {
            return await Task.FromResult($"You said: '{greeting}', I say: Hello");
        }
    }
}
