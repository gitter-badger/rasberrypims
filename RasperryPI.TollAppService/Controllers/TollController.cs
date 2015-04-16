using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RasperryPI.Framework;

namespace RasperryPI.TollAppService.Controllers
{
    public class TollController : ApiController
    {
        private readonly IAzureQueueService _queueService;
        private readonly BlobFileManager _fileManager;

        public TollController()
        {
            _queueService = new AzureQueueService(new AzureQueueRepository(new AzureQueueFactory()));
            _fileManager = new BlobFileManager(
                TollConfiguration.GetConfigurationItem("imagecontainer"),
                TollConfiguration.GetConfigurationItem("storageconnectionstring"));
        }

        public TollController(IAzureQueueService queueService)
        {
            _queueService = queueService;
        }

        [Route("toll")]
        [HttpPost]
        public IHttpActionResult Post(string deviceId, string geoLocation, DateTime? captureTime = null, string vehicleNo = "")
        {
            try
            {
                var message = new TollMessage { DeviceId = deviceId, CaptureTime = captureTime.HasValue ? captureTime.Value : DateTime.Now, GeoLocation = geoLocation };

                // if vehicleNo is empty upload image to cloud.
                if (string.IsNullOrEmpty(vehicleNo))
                {
                    var image = Request.Content.ReadAsStreamAsync().Result;
                    var url = string.Format(TollConfiguration.GetConfigurationItem("imgurl"), _fileManager.PersisteFile(image, "deviceId"));
                    message.ImageUrl = url;
                }

                _queueService.AddTollMessage(message);

                return Ok(true);
            }
            catch (Exception e)
            {
                //var message = new TollMessage { DeviceId = deviceId, CaptureTime = captureTime.HasValue ? captureTime.Value : DateTime.Now, GeoLocation = geoLocation };

                //_queueService.AddTollMessage(message);
                return Ok(e.ToString());
            }
        }

        [Route("toll2")]
        [HttpPost]
        public HttpResponseMessage Post2(string deviceId, string geoLocation, DateTime? captureTime = null, string vehicleNo = "")
        {
            try
            {
                var message = new TollMessage { DeviceId = deviceId, CaptureTime = captureTime.HasValue ? captureTime.Value : DateTime.Now, GeoLocation = geoLocation };

                // if vehicleNo is empty upload image to cloud.
                if (string.IsNullOrEmpty(vehicleNo))
                {
                    var image = Request.Content.ReadAsStreamAsync().Result;
                    var url = string.Format(TollConfiguration.GetConfigurationItem("imgurl"), _fileManager.PersisteFile(image, "deviceId"));
                    message.ImageUrl = url;
                }

                _queueService.AddTollMessage(message);

                return Request.CreateResponse(HttpStatusCode.OK, "successful");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, e.ToString());
            }
        }
    }
}
