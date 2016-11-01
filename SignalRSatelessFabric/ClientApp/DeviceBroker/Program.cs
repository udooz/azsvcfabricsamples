using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMock
{
    class Program
    {
        static void Main(string[] args)
        {
            string deviceID = (args.Count() > 0) ? args[0] : "device1";

            
            var connection = new Connection("http://localhost:9191/echo", "deviceId=" + deviceID);

            //var connection = new Connection("http://10.1.167.72:9090/notifyserviceFabric", "deviceId=" + deviceID);

            connection.Start().Wait();

            connection.Received += data =>
            {
                Console.WriteLine(data);
            };

            Console.ReadKey();
        }
    }
}
