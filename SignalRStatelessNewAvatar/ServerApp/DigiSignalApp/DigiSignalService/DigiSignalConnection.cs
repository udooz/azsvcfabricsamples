using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DigiSignalService
{
    public class DigiSignalConnection : PersistentConnection
    {
        protected override async Task OnConnected(IRequest request, string connectionId)
        {
            string deviceID = request.QueryString["deviceId"].ToLowerInvariant();

            TableStorageHelper tableStorage = new TableStorageHelper();
            var devEntity = new DeviceEntity("device", deviceID) { ConnectionID = connectionId };
            tableStorage.AddOfUpdateDevice(devEntity);

            await Connection.Broadcast("Connection " + connectionId + " connected");
        }

        protected override async Task OnReconnected(IRequest request, string connectionId)
        {
            await Connection.Broadcast("Client " + connectionId + " re-connected");
        }

        protected override async Task OnReceived(IRequest request, string connectionId, string data)
        {
            await Connection.Broadcast("Connection " + connectionId + " sent " + data);
        }

        protected override async Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            await base.OnDisconnected(request, connectionId, stopCalled);
        }

    }
}
