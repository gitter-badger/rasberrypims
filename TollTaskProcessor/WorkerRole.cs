using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Aspose.OCR;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using RasperryBI.CitizenDataService.Entities;
using RasperryBI.RTADataService.Entities;
using RasperryBI.TollAggregatorService.Entities;
using RasperryPI.Framework;
using asprise_ocr_api;

namespace TollTaskProcessor
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("TollTaskProcessor is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();

            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("TollTaskProcessor has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TollTaskProcessor is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("TollTaskProcessor has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var azureQueueService = new AzureQueueService(new AzureQueueRepository(new AzureQueueFactory()));

            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                var tollnmessage = azureQueueService.ReadTollMessage();
                try
                {
                    if (tollnmessage != null)
                    {
                        var toll = new Toll();
                        toll.TollImageUrl = tollnmessage.ImageUrl;
                        //Initialize an instance of OcrEngine


                        //Set the Image property by loading the image from file path location or an instance of MemoryStream
                        var blob = new BlobFileManager("tollimages",
                            TollConfiguration.GetConfigurationItem("storageconnectionstring"));
                        //var webClient = new WebClient();
                        //byte[] imageBytes = webClient.DownloadData(tollnmessage.ImageUrl);
                        //MemoryStream stream = blob.ReadStreamImage(tollnmessage.ImageUrl) as MemoryStream;

                        ////Initialize an instance of OcrEngine
                        //OcrEngine ocrEngine = new OcrEngine();

                        ////Set Image property by loading an image from file path
                        //ocrEngine.Image = ImageStream.FromMemoryStream(stream, ImageStreamFormat.Jpg);
                        //string processedText = "";

                        //if (ocrEngine.Process())
                        //{
                        //    processedText = ocrEngine.Text != null ? ocrEngine.Text.ToString() : "Vehicle Not Found";
                        //}
                        // read the toll message.

                        toll.VehicleNo = "AP 09 7660";

                        string citizneId = null;
                        // get the vehicle info.
                        using (var client = new HttpClient())
                        {
                            //string deviceId, string vehicleNo, DateTime captureTime, string geoLocation
                            var rtaResponse = client.GetAsync("http://rtatoll.azurewebsites.net/rta/" + toll.VehicleNo).Result;
                            string responseBody = await rtaResponse.Content.ReadAsStringAsync();
                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            var result = json_serializer.Deserialize<RTA>(responseBody);
                            Debug.WriteLine(result.OwnerUniqueId);
                            toll.VehicleModel = result.Model;
                            // get the citizen info.
                            citizneId = result.OwnerUniqueId;
                            // add the toll info.
                        }

                        // get the vehicle info.
                        using (var client = new HttpClient())
                        {
                            //string deviceId, string vehicleNo, DateTime captureTime, string geoLocation
                            var rtaResponse = client.GetAsync("http://cdstoll.azurewebsites.net/citizens/" + citizneId).Result;
                            string responseBody = await rtaResponse.Content.ReadAsStringAsync();
                            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                            var result = json_serializer.Deserialize<Citizen>(responseBody);
                            Debug.WriteLine(result.CitizenName);

                            // get the citizen info.
                            toll.CitizenName = result.CitizenName;
                            // add the toll info.
                        }

                        // get the vehicle info.
                        using (var client = new HttpClient())
                        {
                            var values = new Dictionary<string, string>(){
                                {"TollImageUrl", tollnmessage.ImageUrl.Replace(".png",".jpg")},
                                {"CitizenName",  toll.CitizenName},
                                {"VehicleModel", toll.VehicleModel},
                                {"VehicleNo", toll.VehicleNo}
                                };

                            var content = new FormUrlEncodedContent(values);
                            //string deviceId, string vehicleNo, DateTime captureTime, string geoLocation
                            var rtaResponse = client.PostAsync("http://tastoll.azurewebsites.net/post", content).Result;
                            rtaResponse.EnsureSuccessStatusCode();
                            // add the toll info.
                        }

                    }
                }
                catch (Exception exception)
                {
                    Trace.TraceError(exception.ToString());
                }
                
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
