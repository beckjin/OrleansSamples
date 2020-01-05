using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHelloGrain : IGrainWithIntegerKey
    {
        Task<string> SayHelloAsync(string greeting);
    }
}
