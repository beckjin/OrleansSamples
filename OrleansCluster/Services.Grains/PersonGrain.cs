using Orleans;
using Orleans.Providers;
using Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.Grains
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
