using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COFoundation
{
    public class MessageCarrier<T>
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public T Value {get;set;}
    }
}
