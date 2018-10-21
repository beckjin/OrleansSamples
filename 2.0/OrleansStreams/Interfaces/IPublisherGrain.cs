using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPublisherGrain : IGrainWithGuidKey
    {
        Task PublishMessageAsync(string data);
    }
}
