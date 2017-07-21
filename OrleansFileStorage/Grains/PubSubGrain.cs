using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class PubSubGrain : Grain, IPubSubGrain
    {

        ObserverSubscriptionManager<IFooGrainObserver> subscribers;

        public override async Task OnActivateAsync()
        {
            subscribers = new ObserverSubscriptionManager<IFooGrainObserver>();
            await base.OnActivateAsync();
        }

        public Task Subscribe(IFooGrainObserver observer)
        {
            subscribers.Subscribe(observer);
            return Task.CompletedTask;
        }

        public Task UnSubscribe(IFooGrainObserver observer)
        {
            subscribers.Unsubscribe(observer);
            return Task.CompletedTask;
        }

        public Task Publish(string message)
        {
            subscribers.Notify(x => x.Foo(message));
            return Task.CompletedTask;
        }

       
    }
}
