using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class SubscriberGrain : Grain, ISubscriberGrain
    {
        public override async Task OnActivateAsync()
        {
            var guid = this.GetPrimaryKey();
            var streamProvider = this.GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<string>(guid, "GrainStream");
            await stream.SubscribeAsync(
                (payload, token) => this.ReceiveMessageAsync(payload)
            );

            await base.OnActivateAsync();
        }

        public Task ReceiveMessageAsync(string payload)
        {
            Console.WriteLine($"Message Received: {payload}");
            return Task.CompletedTask;
        }

        public Task SubscribeAsync()
        {
            return Task.CompletedTask;
        }
    }

}
