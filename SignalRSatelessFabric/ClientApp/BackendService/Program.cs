using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BackendService
{
    class Program
    {
        static void Main(string[] args)
        {

            string deviceID = (args.Count() > 0) ? args[0] : "device1";
            string message = string.IsNullOrEmpty(args[1]) ? "Hello " : args[1];

            Console.WriteLine(deviceID);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8401");

            var res = client.GetStringAsync($"api/values/send?deviceId={deviceID}&message={message}").Result;
            Console.WriteLine(res);
        }

        public class DeviceMessage
        {
           public string DeviceID { get; set; }
        }

    }
}
