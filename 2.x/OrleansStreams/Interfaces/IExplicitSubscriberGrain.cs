using Orleans;
using Orleans.Streams;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IExplicitSubscriberGrain : IGrainWithGuidKey
    {
        Task<StreamSubscriptionHandle<string>> SubscribeAsync();

        Task ReceivedMessageAsync(string data);
    }
}
