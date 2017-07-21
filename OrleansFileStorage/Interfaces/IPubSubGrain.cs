using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPubSubGrain : IGrain
    {
        Task Subscribe(IFooGrainObserver observer);
        Task UnSubscribe(IFooGrainObserver observer);
        Task Publish(string message);
    }
}
