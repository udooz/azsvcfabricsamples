using ContextActorService.Interfaces;
using ContextService;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace COClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var greet = new HelloMessage { Greet = "Hello" };
            var proxy = ActorProxy.Create<IContextActorService>(new ActorId(Guid.NewGuid().ToString()), new Uri("fabric:/ContextObjectApp/ContextActorServiceActorService"));
            var result = proxy.ConsumeContext(new COFoundation.MessageCarrier<HelloMessage>
            {
                DeviceId = "D1",
                UserId = "U1",
                Value = greet
            });

            
            

            var proxy2 = ActorProxy.Create<IContextActorService>(new ActorId(Guid.NewGuid().ToString()), new Uri("fabric:/ContextObjectApp/ContextActorServiceActorService"));
            var result2 = proxy.ConsumeContext(new COFoundation.MessageCarrier<HelloMessage>
            {
                DeviceId = "D2",
                UserId = "U2",
                Value = greet
            });

            result.Wait();            

            result2.Wait();

            Console.WriteLine(result.Result);
            Console.WriteLine(result2.Result);

            result = proxy.ConsumeContext(new COFoundation.MessageCarrier<HelloMessage>
            {
                DeviceId = "D1",
                UserId = "U3",
                Value = greet
            });

            result.Wait();
            Console.WriteLine(result.Result);

            Console.ReadKey();
        }
    }
}
