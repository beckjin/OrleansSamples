using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace Grains
{
    [ImplicitStreamSubscription("GrainStream1")]
    public class ImplicitSubscriberGrain : Grain, ISubscriberGrain, IAsyncObserver<string>
    {
        protected StreamSubscriptionHandle<string> streamHandle;

        public override async Task OnActivateAsync()
        {
            var guid = this.GetPrimaryKey();
            var streamProvider = this.GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<string>(guid, "GrainStream1");
            streamHandle = await stream.SubscribeAsync(OnNextAsync);
        }

        public override async Task OnDeactivateAsync()
        {
            if (streamHandle != null)
                await streamHandle.UnsubscribeAsync();
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        public Task OnNextAsync(string item, StreamSequenceToken token = null)
        {
            Console.WriteLine($"Received message:{item}");
            return Task.CompletedTask;
        }
    }
}
