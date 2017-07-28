using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class PublisherGrain : Grain, IPublisherGrain
    {
        private IAsyncStream<string> grainStream;

        public override Task OnActivateAsync()
        {
            var guid = this.GetPrimaryKey();
            var streamProvider = this.GetStreamProvider("SMSProvider");
            this.grainStream = streamProvider.GetStream<string>(guid, "GrainStream");
            return base.OnActivateAsync();
        }

        public async Task PublishMessageAsync(string data)
        {
            Console.WriteLine($"Sending data: {data}");
            await this.grainStream.OnNextAsync(data);
        }
    }
}
