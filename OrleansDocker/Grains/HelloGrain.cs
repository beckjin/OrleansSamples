using Interfaces;
using System.Threading.Tasks;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHelloGrain
    {
        Task<string> IHelloGrain.SayHello(string greeting)
        {
            return Task.FromResult($"You said: '{greeting}', I say: Hello!");
        }
    }
}
