using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors.Client;
using Masha.Quote.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ApplicationInsights;

namespace Mash.Sales.Controllers
{
    [Produces("application/json")]
    [Route("api/Sales")]
    public class SalesController : Controller
    {
        private TelemetryClient tclient = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration("d213c5f7-d154-4ccf-8dd4-726cf2023a5a"));
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            tclient.TrackRequest("SalesGet", DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(10), "200", true);
            tclient.TrackEvent("Sales.Query");
            return new string[] { "Books", "Toys" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            tclient.TrackEvent("Sales.Query");
            return "Toys";
        }

        // POST api/values
        [HttpPost]
        public string Post(string sku)
        {
            tclient.TrackEvent("Sales.Quote");
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