using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHelloGrain : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
