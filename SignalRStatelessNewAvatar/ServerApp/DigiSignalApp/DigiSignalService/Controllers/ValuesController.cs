using System.Collections.Generic;
using System.Web.Http;

namespace DigiSignalService.Controllers
{
    [ServiceRequestActionFilter]
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [HttpGet]
        // POST api/values 
        public string Send(string message, string deviceId)
        {
            Notifier notify = new Notifier();
            notify.Notify(deviceId, message).Wait();
            return "done";
        }
    }
}
