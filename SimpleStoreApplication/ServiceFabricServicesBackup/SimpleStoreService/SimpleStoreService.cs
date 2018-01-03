﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using SimpleStoreCommon;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace SimpleStoreService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SimpleStoreService : StatefulService, IShoppingCartService
    {
        public SimpleStoreService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddItem(ShoppingCartItem item)
        {
            item.Description = $"Served from {this.Context.PartitionId} partition by {this.Context.NodeContext.NodeName} node";
            var cart = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, ShoppingCartItem>>("mycart");
            using (var tx = this.StateManager.CreateTransaction())
            {
                await cart.AddOrUpdateAsync(tx, item.ProductName, item, (k, v) => item);
                await tx.CommitAsync();
            }
        }

        public async Task DeleteItem(string productName)
        {
            var cart = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, ShoppingCartItem>>("mycart");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var existing = await cart.TryGetValueAsync(tx, productName);
                if(existing.HasValue)
                {
                    await cart.TryRemoveAsync(tx, productName);
                }
                await tx.CommitAsync();
            }
        }

        public async Task<List<ShoppingCartItem>> GetItems()
        {
            var ret = await QueryReliableDictionary<ShoppingCartItem>(this.StateManager, "mycart");
            return ret.ToList();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see http://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        //protected override async Task RunAsync(CancellationToken cancellationToken)
        //{
        //    // TODO: Replace the following sample code with your own logic 
        //    //       or remove this RunAsync override if it's not needed in your service.

        //    var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

        //    while (true)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();

        //        using (var tx = this.StateManager.CreateTransaction())
        //        {
        //            var result = await myDictionary.TryGetValueAsync(tx, "Counter");

        //            ServiceEventSource.Current.ServiceMessage(this, "Current Counter Value: {0}",
        //                result.HasValue ? result.Value.ToString() : "Value does not exist.");

        //            await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

        //            // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
        //            // discarded, and nothing is saved to the secondary replicas.
        //            await tx.CommitAsync();
        //        }

        //        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        //    }
        //}
        public static async Task<IList<T>> QueryReliableDictionary<T>(IReliableStateManager stateManager, string collectionName, Func<T, bool> filter = null)
        {
            var result = new List<T>();

            IReliableDictionary<string, T> reliableDictionary =
                await stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(collectionName);

            using (ITransaction tx = stateManager.CreateTransaction())
            {
                IAsyncEnumerable<KeyValuePair<string, T>> asyncEnumerable = await reliableDictionary.CreateEnumerableAsync(tx);
                using (IAsyncEnumerator<KeyValuePair<string, T>> asyncEnumerator = asyncEnumerable.GetAsyncEnumerator())
                {
                    while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                    {                        
                        if (filter == null || filter(asyncEnumerator.Current.Value))
                            result.Add(asyncEnumerator.Current.Value);
                    }
                }
            }
            return result;
        }
    }    
}
