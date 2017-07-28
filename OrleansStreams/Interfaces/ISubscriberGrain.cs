using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ISubscriberGrain : IGrainWithGuidKey
    {
        Task ReceiveMessageAsync(string data);

        Task SubscribeAsync();
    }
}