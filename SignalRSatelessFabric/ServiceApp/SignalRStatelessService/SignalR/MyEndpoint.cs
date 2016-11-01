using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SignalRStatelessService.SignalR
{
    public class MyEndPoint : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            string deviceID = request.QueryString["deviceId"].ToLowerInvariant();

            TableStorageHelper tableStorage = new TableStorageHelper();
            var devEntity = new DeviceEntity("device", deviceID) { ConnectionID = connectionId };
            tableStorage.AddOfUpdateDevice(devEntity);

            return Connection.Broadcast("Connection " + connectionId + " connected");
        }

        protected override Task OnReconnected(IRequest request, string connectionId)
        {
            return Connection.Broadcast("Client " + connectionId + " re-connected");
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Connection.Broadcast("Connection " + connectionId + " sent " + data);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            return base.OnDisconnected(request, connectionId, stopCalled);
        }

    }
}
