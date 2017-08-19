
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class PersonController : ApiController
    {
        [HttpGet]
        public string SayHello(string name)
        {
            return new Business.Persion().SayHello(name);
        }
    }
}
