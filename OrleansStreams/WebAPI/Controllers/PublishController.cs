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
        /// /api/publish/sendMessage?message=Hello
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public string SendMessage(string message)
        {
            var publisherGrain = GrainClient.GrainFactory.GetGrain<IPublisherGrain>(Guid.Empty);
            publisherGrain.PublishMessageAsync(message);
            return "success";
        }
    }
}
