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
            ServiceEventSource.Current.Message($"{a} + {b}");
            return a + b;
        }
    }
}
