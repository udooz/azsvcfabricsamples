using SignalRStatelessService.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebCalculatorService.Controllers
{
    public class DefaultController : ApiController
    {
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
            notify.Notify(deviceId, message);
            return "done";
        }
    }
}
