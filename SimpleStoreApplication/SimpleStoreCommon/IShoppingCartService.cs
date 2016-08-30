using Microsoft.ServiceFabric.Services.Remoting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleStoreCommon
{
    public interface IShoppingCartService : IService
    {
        Task AddItem(ShoppingCartItem item);
        Task DeleteItem(string productName);
        Task<List<ShoppingCartItem>> GetItems();
    }
}
