using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RasperryBI.CitizenDataService.Data;

namespace RasperryPI.CitizenDataService.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void GetCitizens()
        {
            var repo = new CitizenRepository(new CitizenContext());
            Assert.IsNotNull(repo.GetCitizen("Srini"));
        }
    }
}
