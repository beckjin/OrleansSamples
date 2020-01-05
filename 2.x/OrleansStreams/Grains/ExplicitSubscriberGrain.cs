using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Grains
{
    /// <summary>
    /// 显示订阅
    /// </summary>
    public class ExplicitSubscriberGrain : Grain, IExplicitSubscriberGrain
    {
        private IAsyncStream<string> stream;

        public async override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");
            stream = streamProvider.GetStream<string>(this.GetPrimaryKey(), "GrainExplicitStream");

            // 当 Grain 由未激活状态变为激活状态的时候，通过 GetAllSubscriptionHandles 获取这个 Stream 中存在的订阅者，通过 ResumeAsync 可以把它们重新唤醒
            var subscriptionHandles = await stream.GetAllSubscriptionHandles();
            if (subscriptionHandles.Count > 0)
            {
                subscriptionHandles.ToList().ForEach(async x =>
                {
                    await x.ResumeAsync((payload, token) => ReceivedMessageAsync(payload));
                });
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <returns></returns>
        public async Task<StreamSubscriptionHandle<string>> SubscribeAsync()
        {
            return await stream.SubscribeAsync((payload, token) => ReceivedMessageAsync(payload));
        }

        public Task ReceivedMessageAsync(string data)
        {
            Console.WriteLine($"Received message:{data}");
            return Task.CompletedTask;
        }
    }
}
