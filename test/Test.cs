
#r "Microsoft.Azure.EventHubs"
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public class Test

{
        

 
private static CloudBlobClient blobClient;

public static void Run(EventData myEventHubMessage, ILogger log)
{
    log.LogInformation($
      "C# Event Hub trigger function processed a message: {
  
          Encoding.UTF8.GetString(myEventHubMessage.Body)}
");
  log.LogInformation($"Message = {Encoding.UTF8.GetString(myEventHubMessage.Body)}");
foreach (KeyValuePair<string, object> props in myEventHubMessage.Properties)
{
    log.LogInformation($"Properties Key = {props.Key }");
    log.LogInformation($"Properties Value = {props.Value}");
}
log.LogInformation($
      "System Properties EnqueuedTimeUtc = {

    myEventHubMessage.SystemProperties.EnqueuedTimeUtc}");
  log.LogInformation($"System Properties Offset = {myEventHubMessage.SystemProperties.Offset}");
log.LogInformation($"System Properties PartitionKey = {

    myEventHubMessage.SystemProperties.PartitionKey}");
  log.LogInformation($
      "System Properties SequenceNumber = {myEventHubMessage.SystemProperties.SequenceNumber}");
try
{
    var blobConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    log.LogInformation($"The blob connection string is: {blobConnectionString}");
    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnectionString);
    blobClient = storageAccount.CreateCloudBlobClient();
    CloudBlobContainer blobContainer = blobClient.GetContainerReference("csharpguitar");
    log.LogInformation($"The blob name is: {Encoding.UTF8.GetString(myEventHubMessage.Body)}.txt");
    CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference($
      "{Encoding.UTF8.GetString(myEventHubMessage.Body)}.txt");
    log.LogInformation($
      "The blob content is: Hello, World! It is currently {
      myEventHubMessage.SystemProperties.EnqueuedTimeUtc}");
    blockBlob.UploadTextAsync($
      "Hello, World! It is currently {myEventHubMessage.SystemProperties.EnqueuedTimeUtc}");
log.LogInformation($"The blob has been uploaded.");
  }
  catch (StorageException se)
{
    log.LogInformation($"StorageException: {se.Message}");
}
catch (Exception ex)
{
    log.LogInformation($"{DateTime.Now} > Exception: {ex.Message}");
}    
}

}
