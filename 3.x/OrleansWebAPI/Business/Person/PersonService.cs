using Interfaces;
using Orleans;

namespace Business.Person
{
    public class PersonService: IPersonService
    {
        private readonly IClusterClient _orleansClient;

        public PersonService(IClusterClient orleansClient)
        {
            _orleansClient = orleansClient;
        }

        public string SayHello(string name)
        {
            var grain = _orleansClient.GetGrain<IPersonGrain>(name);
            grain.SayHelloAsync();
            return "success";
        }
    }
}
