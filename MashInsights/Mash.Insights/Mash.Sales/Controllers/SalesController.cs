using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors.Client;
using Masha.Quote.Interfaces;
using Microsoft.ServiceFabric.Actors;

namespace Mash.Sales.Controllers
{
    [Produces("application/json")]
    [Route("api/Sales")]
    public class SalesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Books", "Toys" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Toys";
        }

        // POST api/values
        [HttpPost]
        public string Post(string sku)
        {
            var actorProxy = ActorProxy.Create<IQuote>(ActorId.CreateRandom(), "fabric:/Mash.Insights");
            var result = actorProxy.GetQuote(sku).Result;
            return $"Price for {sku} is {result}";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}