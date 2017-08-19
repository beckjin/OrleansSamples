using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITest : Orleans.IGrainWithIntegerKey
    {
        Task AddCount(string taskName);
    }
}
