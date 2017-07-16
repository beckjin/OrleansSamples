using Actors.Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Grains
{
    public class Manager : Grain, IManager
    {
        private IEmployee _me;
        private List<IEmployee> _reports = new List<IEmployee>();

        public override Task OnActivateAsync()
        {
            _me = this.GrainFactory.GetGrain<IEmployee>(this.GetPrimaryKey());
            return base.OnActivateAsync();
        }

        public async Task AddDirectReport(IEmployee employee)
        {
            _reports.Add(employee);
            await employee.SetManager(this);
            await employee.Greeting(new GreetingData
            {
                From = this.GetPrimaryKey(),
                Message = "Welcome to my team!"
            });
        }

        public Task<IEmployee> AsEmployee()
        {
            return Task.FromResult(_me);
        }

        public Task<List<IEmployee>> GetDirectReports()
        {
            return Task.FromResult(_reports);
        }
    }
}
