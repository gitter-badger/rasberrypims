using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RasperryBI.TollAggregatorService.Entities;
using RasperryPI.TollAggregatorService.Controllers;

namespace RasperryPI.TollAggregatorService.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var contl = new TollAggregatorController();
            var result = contl.GetTolls("srini") as OkNegotiatedContentResult<JsonResult<List<Toll>>>;
            Assert.IsNotNull(result);
           // Assert.AreEqual(true, result.Content.Count == 2);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var contl = new TollAggregatorController();
            var result = contl.Post(new Toll()
            {
                CitizenName = "test7",
                TollImageUrl = "dsd9sd"
            }) as OkNegotiatedContentResult<bool>;

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Content);
        }

        [TestMethod]
        public void HttpRequest()
        {

            ////http://tapstoll.azurewebsites.net/toll/
            //var client = new HttpClient();
            //var docFile = File.OpenRead(@"C:\Users\srinipi\Downloads\10Valid Records (1).xlsx");
            //var content = new StreamContent(docFile);
            ////string deviceId, string vehicleNo, DateTime captureTime, string geoLocation
            //var res = client.PostAsync("http://tapstoll.azurewebsites.net/toll?deviceId=srini&geoLocation=test&captureTime=2010-10-10", content).Result;

            //Assert.IsNotNull(res.IsSuccessStatusCode);

            // get the vehicle info.
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>(){
                                {"TollImageUrl", "ImageUrl"},
                                {"CitizenName",  "CitizenName"},
                                {"VehicleModel", "VehicleModel"},
                                {"VehicleNo","VehicleNo"}
                                };

                var content = new FormUrlEncodedContent(values);
                //string deviceId, string vehicleNo, DateTime captureTime, string geoLocation
                var rtaResponse = client.PostAsync("http://tastoll.azurewebsites.net/post", content).Result;
                rtaResponse.EnsureSuccessStatusCode();
                // add the toll info.
            }
        }
    }
}
