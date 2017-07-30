using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class PublisherGrain : Grain, IPublisherGrain
    {
        private IAsyncStream<string> stream;

        public override Task OnActivateAsync()
        {
            var streamId = this.GetPrimaryKey();
            var streamProvider = this.GetStreamProvider("SMSProvider");
            this.stream = streamProvider.GetStream<string>(streamId, "GrainExplicitStream"); //隐式：GrainImplicitStream
            return base.OnActivateAsync();
        }

        public async Task PublishMessageAsync(string data)
        {
            Console.WriteLine($"Sending data: {data}");
            await this.stream.OnNextAsync(data);
        }
    }
}
