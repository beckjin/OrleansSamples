using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITestGrain : Orleans.IGrainWithIntegerKey
    {
        Task AddCount(string taskName);
    }
}
