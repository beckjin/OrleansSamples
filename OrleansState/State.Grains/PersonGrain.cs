using Orleans;
using State.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State.Grains
{
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
