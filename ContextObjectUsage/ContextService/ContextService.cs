using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextService
{
    public class CContextService
    {
        public string CreateContext(HelloMessage msg)
        {
            return $"#{msg.Greet}, #{COFoundation.AppContext.Current.UserId}";
        }
    }

    public class HelloMessage
    {
        public string Greet { get; set; }
    }
}
