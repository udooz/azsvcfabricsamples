using System.Collections.Generic;
using System.Web.Http;

namespace GreetApiService.Controllers
{
    [ServiceRequestActionFilter]
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello", "Ola" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }
    }
}
