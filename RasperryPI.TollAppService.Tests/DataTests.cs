﻿using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RasperryPI.TollAppService.Controllers;

namespace RasperryPI.TollAppService.Tests
{
    [TestClass]
    public class ApiTests
    {
        public static void UploadFilesToRemoteUrl(string url, string[] files, string
            logpath, NameValueCollection nvc)
        {

            long length = 0;
            string boundary = "----------------------------" +
            DateTime.Now.Ticks.ToString("x");


            HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest2.ContentType = "multipart/form-data; boundary=" +
            boundary;
            httpWebRequest2.Method = "POST";
            httpWebRequest2.KeepAlive = true;
            httpWebRequest2.Credentials =
            System.Net.CredentialCache.DefaultCredentials;


            Stream memStream = new System.IO.MemoryStream();

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
            boundary + "\r\n");


            string formdataTemplate = "\r\n--" + boundary +
            "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            memStream.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

            for (int i = 0; i < files.Length; i++)
            {

                //string header = string.Format(headerTemplate, "file" + i, files[i]);
                string header = string.Format(headerTemplate, "uplTheFile", files[i]);

                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);


                FileStream fileStream = new FileStream(files[i], FileMode.Open,
                FileAccess.Read);
                byte[] buffer = new byte[1024];

                int bytesRead = 0;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);

                }


                memStream.Write(boundarybytes, 0, boundarybytes.Length);


                fileStream.Close();
            }

            httpWebRequest2.ContentLength = memStream.Length;

            Stream requestStream = httpWebRequest2.GetRequestStream();

            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();


            WebResponse webResponse2 = httpWebRequest2.GetResponse();

            Stream stream2 = webResponse2.GetResponseStream();
            StreamReader reader2 = new StreamReader(stream2);

            Debug.WriteLine(reader2.ReadToEnd());

            webResponse2.Close();
            httpWebRequest2 = null;
            webResponse2 = null;
        }

        [TestMethod]
        public void PostToll()
        {
            var controller = new TollController();
            //var result = controller.Post("Srini", "Srini", DateTime.Now, "Srini") as OkNegotiatedContentResult<bool>;
            //Assert.IsNotNull(result);
            //Assert.AreEqual(true, result.Content);

            UploadFilesToRemoteUrl("http://tapstoll.azurewebsites.net/toll?deviceId=srini&geoLocation=test&captureTime=2010-10-10", new string[1] { @"C:\Users\srinipi\Downloads\10Valid Records (1).xlsx" }, null,
                null);
        }

        [TestMethod]
        public void PostTollWithoutVehicleNo()
        {
            //var controller = new TollController();
            //controller.Request = new HttpRequestMessage();
            //var docFile = File.OpenRead(@"C:\Users\srinipi\Downloads\10Valid Records (1).xlsx");
            //controller.Request.Content = new StreamContent(docFile);
            //var result = controller.Post("Srini", "Srini", DateTime.Now, "Srini") as OkNegotiatedContentResult<bool>;
            //Assert.IsNotNull(result);
            //Assert.AreEqual(true, result.Content);

        }

        [TestMethod]
        public void HttpRequest()
        {

            //http://tapstoll.azurewebsites.net/toll/
            var client = new HttpClient();
            var docFile = File.OpenRead(@"C:\Users\srinipi\Pictures\Capture.PNG");
            var content = new StreamContent(docFile);
            //string deviceId, string vehicleNo, DateTime captureTime, string geoLocation
            var res = client.PostAsync("http://tapstoll.azurewebsites.net/toll?deviceId=srini&geoLocation=test&captureTime=2010-10-10", content).Result;

            Assert.IsNotNull(res.IsSuccessStatusCode);
        }
    }
}
