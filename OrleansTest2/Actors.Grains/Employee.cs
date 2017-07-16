using Actors.Interfaces;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Grains
{
    [Reentrant]
    public class Employee : Grain, IEmployee
    {
        private int _level;
        private IManager _manager;

        public Task<int> GetLevel()
        {
            return Task.FromResult(_level);
        }

        public Task Promote(int newLevel)
        {
            _level = newLevel;
            return TaskDone.Done;
        }

        public Task<IManager> GetManager()
        {
            return Task.FromResult(_manager);
        }

        public Task SetManager(IManager manager)
        {
            _manager = manager;
            return TaskDone.Done;
        }

        public async Task Greeting(GreetingData data)
        {
            Console.WriteLine("{0} said: {1}", data.From, data.Message);

            // stop this from repeating endlessly
            if (data.Count >= 3) return;

            // send a message back to the sender
            var fromGrain = GrainFactory.GetGrain<IEmployee>(data.From);
            await fromGrain.Greeting(new GreetingData
            {
                From = this.GetPrimaryKey(),
                Message = "Thanks!",
                Count = data.Count + 1
            });
        }
    }
}
