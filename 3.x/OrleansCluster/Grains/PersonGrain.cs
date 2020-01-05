using Interfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class PersonGrain : Grain, IPersonGrain
    {
        private readonly IPersistentState<PersonState> _person;

        public PersonGrain([PersistentState("person", "OrleansStorage")] IPersistentState<PersonState> person)
        {
            _person = person;
        }

        public async Task SayHelloAsync()
        {
            string primaryKey = this.GetPrimaryKeyString();

            bool saidHelloBefore = _person.State.SaidHello;
            string saidHelloBeforeStr = saidHelloBefore ? " already" : null;

            Console.WriteLine($"{primaryKey}{saidHelloBeforeStr} said hello!");

            _person.State.SaidHello = true;
            await _person.WriteStateAsync();
        }
    }

    public class PersonState
    {
        public bool SaidHello { get; set; }
    }
}
