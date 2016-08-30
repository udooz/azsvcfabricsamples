using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using SimpleStoreCommon;
using System;

namespace SimpleStoreClient
{
    class Program
    {
        static void Main(string[] args)
        {            
            ServicePartitionKey pk = new ServicePartitionKey(9223372036854775807);            

            var simpleStoreClient = ServiceProxy.Create<IShoppingCartService>
                (new Uri("fabric:/SimpleStoreApplication/SimpleStoreService"), pk);
            
            simpleStoreClient.AddItem(new ShoppingCartItem
            {
                ProductName = "XBOX ONE",
                UnitPrice = 329.0,
                Amount = 2
            }).Wait();

            var list = simpleStoreClient.GetItems().Result;
            foreach (var item in list)
            {
                Console.WriteLine(string.Format("{0}: {1:C2} X {2} = {3:C2}",
                item.ProductName,
                item.UnitPrice,
                item.Amount,
                item.LineTotal));
            }
            Console.ReadKey();
        }
    }
}
