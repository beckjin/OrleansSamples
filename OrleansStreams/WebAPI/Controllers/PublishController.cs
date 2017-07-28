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
        /// <summary>
        /// /api/publish/sendMessage?message=hi
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public Task SendMessage(string message)
        {
            var subscriberGrain = GrainClient.GrainFactory.GetGrain<IRandomReceiver>(Guid.Empty);
            subscriberGrain.SendMessage(message);
            return Task.CompletedTask;
        }
    }
}
