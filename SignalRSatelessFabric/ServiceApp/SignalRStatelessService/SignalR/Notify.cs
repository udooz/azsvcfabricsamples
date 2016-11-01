using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SignalRStatelessService.SignalR
{
    public class Notifier
    {
        public void Notify(string deviceID, string message)
        {
          
            var context = GlobalHost.ConnectionManager.GetConnectionContext<MyEndPoint>();

            TableStorageHelper tableStorage = new TableStorageHelper();
            var devEntity = tableStorage.Get("device", deviceID);
            string connID =  devEntity.ConnectionID;
            context.Connection.Send(connID, message);
            //context.Connection.Broadcast(message);

        }
    }
}
