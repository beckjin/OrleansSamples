using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Grains
{
    public class ExplicitSubscriberGrain : Grain, IExplicitSubscriberGrain
    {
        private IAsyncStream<string> stream;

        public async override Task OnActivateAsync()
        {
            var streamProvider = this.GetStreamProvider("SMSProvider");
            stream = streamProvider.GetStream<string>(this.GetPrimaryKey(), "GrainExplicitStream");
            var subscriptionHandles = await stream.GetAllSubscriptionHandles();
            if (subscriptionHandles.Count > 0)
            {
                subscriptionHandles.ToList().ForEach(async x =>
                {
                    await x.ResumeAsync((payload, token) => this.ReceivedMessageAsync(payload));
                });
            }
        }

        public async Task<StreamSubscriptionHandle<string>> SubscribeAsync()
        {
            return await stream.SubscribeAsync((payload, token) => this.ReceivedMessageAsync(payload));
        }

        public Task ReceivedMessageAsync(string data)
        {
            Console.WriteLine($"Received message:{data}");
            return Task.CompletedTask;
        }
    }
}
