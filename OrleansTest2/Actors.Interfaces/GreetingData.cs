using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Interfaces
{
    public class GreetingData
    {
        public Guid From { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
    }
}
