// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoenixAzureStorageHelper.cs" company="Microsoft">
//   All rights reserved for Microsoft.
// </copyright>
// <summary>
//   The PhoenixAzure Storage Helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace RasperryPI.Framework
{
    /// <summary>
    /// The PhoenixAzure Storage Helper.
    /// </summary>
    public class PhoenixAzureStorageHelper
    {
        /// <summary>
        /// The file upload method which uploads the stream to cloud.
        /// </summary>
        /// <param name="fileStream">the file stream.</param>
        /// <param name="referenceId">the reference id for the file to be uploaded.</param>
        /// <param name="connectionString">the connection string for blob storage.</param>
        /// <returns>returns true if file upload is successful or not.</returns>
        public Stream DownloadFile(string containerName, string fileName, string connectionString, Dictionary<string, string> fileMetadata)
        {
            try
            {
                // Get the azure storage account on basis of name and key
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

                // Get the client and container objects
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);
                container.CreateIfNotExists(); // Creates the container if it does not exist

                // Create a blob block for the file
                CloudBlockBlob blobReference = container.GetBlockBlobReference(fileName);
                var ms = new MemoryStream();
                blobReference.DownloadToStream(ms);
                return ms;
            }
            catch (StorageException exception)
            {
               // PhoenixLogger2.LogException(exception);
                throw;
            }
        }

        public void DeleteFile(string containerName, string fileName, string connectionString)
        {
            try
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                blockBlob.DeleteIfExists();
            }
            catch (StorageException exception)
            {
            //    PhoenixLogger2.LogException(exception);
               // throw exception;
            }
           
        }

        /// <summary>
        /// The file upload method which uploads the stream to cloud.
        /// </summary>
        /// <param name="fileStream">the file stream.</param>
        /// <param name="referenceId">the reference id for the file to be uploaded.</param>
        /// <param name="connectionString">the connection string for blob storage.</param>
        /// <returns>returns true if file upload is successful or not.</returns>
        public bool UploadFile(Stream fileStream, string containerName, string fileName, string connectionString, Dictionary<string, string> fileMetadata)
        {
            try
            {
                // Get the azure storage account on basis of name and key
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

                // Get the client and container objects
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(containerName);
                if (container.CreateIfNotExists())
                {
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                // Create a blob block for the file
                CloudBlockBlob blobReference = container.GetBlockBlobReference(fileName);
                foreach (var item in fileMetadata)
                {
                    blobReference.Metadata.Add(item.Key, item.Value);
                }
                fileStream.Position = 0;//Move the pointer to the start of stream.

                // Create or overwrite the "myblob" blob with contents from a local file.

                blobReference.UploadFromStream(fileStream);

                // Execute the upload operation
                // blobReference.UploadFromStream(fileStream);
                return true;
            }
            catch (StorageException exception)
            {
            //    PhoenixLogger2.LogException(exception);
                throw exception;
            }
        }

        /// <summary>
        /// The file upload method which uploads the stream to cloud.
        /// </summary>
        /// <param name="fileStream">the file stream.</param>
        /// <param name="connectionString">the connection string for blob storage.</param>
        /// <returns>returns true if file upload is successful or not.</returns>
        public bool UploadFile(Stream fileStream, string connectionString)
        {
            try
            {
                // Get the azure storage account on basis of name and key
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

                // Get the client and container objects
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("sriniexcelcontainer");
                container.CreateIfNotExists(); // Creates the container if it does not exist

                // Create a blob block for the file
                CloudBlockBlob blobReference = container.GetBlockBlobReference("sriniPricingData.xls");

                // Execute the upload operation
                blobReference.UploadFromStream(fileStream);
                return true;
            }
            catch (StorageException exception)
            {
            //    PhoenixLogger2.LogException(exception);
                return false;
            }
        }
    }
}
