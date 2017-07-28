using Interfaces;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class ReceiverGrain : Grain, IRandomReceiver
    {
        IAsyncStream<string> stream;

        public override Task OnActivateAsync()
        {
            var guid = this.GetPrimaryKey();

            var streamProvider = GetStreamProvider("SMSProvider");
            stream = streamProvider.GetStream<string>(guid, "RANDOMDATA");

            //RegisterTimer(s =>
            //{
            //    return stream.OnNextAsync(new Random().Next().ToString());
            //}, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));

            return base.OnActivateAsync();
        }

        public Task SendMessage(string message)
        {
            return stream.OnNextAsync(message);
        }
    }
}
