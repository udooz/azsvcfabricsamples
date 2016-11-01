using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRStatelessService
{
    public class DeviceEntity : TableEntity
    {
        public DeviceEntity(string partitionkey, string deviceID)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = deviceID;
        }

        public DeviceEntity() { }

        public string ConnectionID { get; set; }


    }
}
