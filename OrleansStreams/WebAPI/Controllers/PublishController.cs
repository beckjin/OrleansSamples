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
        /// /api/publish/sendMessage?message=hi&guid=
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public Task SendMessage(string message, string guid)
        {
            var primaryKey = Guid.Parse(guid);
            var publisherGrain = GrainClient.GrainFactory.GetGrain<IPublisherGrain>(primaryKey);
            publisherGrain.PublishMessageAsync(message);
            return Task.CompletedTask;
        }
    }
}
