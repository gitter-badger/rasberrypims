using System;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RasperryBI.CitizenDataService.Data;
using RasperryBI.CitizenDataService.Entities;
using RasperryPI.CitizenDataService.Controllers;

namespace RasperryPI.CitizenDataService.Tests
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public void GetCitizenValid()
        {
            var controller = new CitizenController(new CitizenRepository(new CitizenContext()));

            var result = controller.GetCitizen("Srini") as OkNegotiatedContentResult<Citizen>;
            Assert.IsNotNull(result);
            Assert.AreEqual("Srini", result.Content.CitizenUniqueId);
        }

        [TestMethod]
        public void GetCitizenInValid()
        {
            var controller = new CitizenController(new CitizenRepository(new CitizenContext()));
            var result = controller.GetCitizen("Srini1") as OkNegotiatedContentResult<Citizen>;
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
