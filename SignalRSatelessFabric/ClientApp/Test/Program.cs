using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            TableStorageHelper helper = new TableStorageHelper();
            helper.AddOfUpdateDevice(new DeviceEntity() { ConnectionID = "connIDtest2", RowKey = "devid1", PartitionKey ="device"  });
        }
    }
}
