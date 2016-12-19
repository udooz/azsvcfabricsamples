using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiSignalService
{
    public class Notifier
    {
        public async Task Notify(string deviceID, string message)
        {

            var context = GlobalHost.ConnectionManager.GetConnectionContext<DigiSignalConnection>();

            TableStorageHelper tableStorage = new TableStorageHelper();
            var devEntity = tableStorage.Get("device", deviceID);
            if (devEntity != null)
            {
                string connID = devEntity.ConnectionID;
                await context.Connection.Send(connID, message);
            }
            //context.Connection.Broadcast(message);

        }
    }
}
