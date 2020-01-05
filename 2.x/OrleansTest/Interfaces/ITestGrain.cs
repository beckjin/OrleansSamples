using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITestGrain : IGrainWithIntegerKey
    {
        Task AddCountAsync(string taskName);
    }
}
