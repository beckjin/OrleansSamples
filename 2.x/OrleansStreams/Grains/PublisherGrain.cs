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
            var streamProvider = GetStreamProvider("SMSProvider");

            // 发布对应的stream命名空间，有隐式或显式类型
            stream = streamProvider.GetStream<string>(streamId, "GrainImplicitStream"); //隐式：GrainImplicitStream
            return base.OnActivateAsync();
        }

        public async Task PublishMessageAsync(string data)
        {
            Console.WriteLine($"Sending data: {data}");
            await stream.OnNextAsync(data);
        }
    }
}
