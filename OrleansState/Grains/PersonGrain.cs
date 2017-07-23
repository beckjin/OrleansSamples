using Orleans;
using Orleans.Providers;
using Interfaces;
using System;
using System.Threading.Tasks;


namespace Grains
{
    [StorageProvider(ProviderName = "OrleansStorage")]
    public class PersonGrain : Grain<PersonGrainState>, IPersonGrain
    {
        public async Task SayHelloAsync()
        {
            string primaryKey = this.GetPrimaryKeyString();

            bool saidHelloBefore = this.State.SaidHello;
            string saidHelloBeforeStr = saidHelloBefore ? " already" : null;

            Console.WriteLine($"{primaryKey}{saidHelloBeforeStr} said hello!");

            this.State.SaidHello = true;
            await this.WriteStateAsync();
        }
    }

    public class PersonGrainState
    {
        public bool SaidHello { get; set; }
    }
}
