using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class TableStorageHelper
    {

        public void AddOfUpdateDevice(DeviceEntity device)
        {
            var table = GetTable();

            var entity = this.Get(device.PartitionKey,device.RowKey);
            if (entity == null)
            {
                TableOperation insertOperation = TableOperation.Insert(device);
                table.Execute(insertOperation);
                
            }
            else
            {
                TableOperation updateOperation = TableOperation.InsertOrMerge(device);
                table.Execute(updateOperation);
            }
        }

            

        public DeviceEntity Get(string partitionKey, string rowKey)
        {
            var table = GetTable();
            TableOperation retrieveOperation = TableOperation.Retrieve<DeviceEntity>(partitionKey, rowKey);
            TableResult retrievedResult = table.Execute(retrieveOperation);
            if (retrievedResult.Result != null)
            {
                return ((DeviceEntity)retrievedResult.Result); 
            }
            else
            {
                return null;
            }
        }

        public CloudTable GetTable()
        {
            string staccount = "DefaultEndpointsProtocol=https;AccountName=lbasdevtest;AccountKey=gRpQjh/FRq93H435+wVZpCANCOP/3k4XvGF5TTKwMbfska0xwVQGB5fafIYd5ZdMVDkHflhlyLioW+2OxfDmMw==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(staccount);
            
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("deviceconnection");
            table.CreateIfNotExists();
            return table;
        }
    }
}
