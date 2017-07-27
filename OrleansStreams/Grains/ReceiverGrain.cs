using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    //[ImplicitStreamSubscription("RANDOMDATA")]
    public class ReceiverGrain : Grain, IRandomReceiver
    {
        public override Task OnActivateAsync()
        {
            var guid = this.GetPrimaryKey();

            var streamProvider = GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<int>(guid, "RANDOMDATA");

            RegisterTimer(s => {
                return stream.OnNextAsync(new Random().Next());
            }, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));

            return base.OnActivateAsync();
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }
    }
}
