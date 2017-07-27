using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class PublishController : ApiController
    {
        [HttpGet]
        public Task ProcessMessage(string message)
        {
            var subscriberGrain = GrainClient.GrainFactory.GetGrain<IRandomReceiver>(new Guid());
            subscriberGrain.SendRandomMessage(message);
            return Task.CompletedTask;
        }
    }
}
