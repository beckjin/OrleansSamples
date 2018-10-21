using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPersonGrain : IGrainWithStringKey
    {
        Task SayHelloAsync();
    }
}