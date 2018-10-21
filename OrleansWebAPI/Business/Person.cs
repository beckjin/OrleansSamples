using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Person
    {
        public string SayHello(string name)
        {
            var grain = GrainClient.GrainFactory.GetGrain<IPersonGrain>(name);
            grain.SayHelloAsync();
            return "success";
        }
    }
}
