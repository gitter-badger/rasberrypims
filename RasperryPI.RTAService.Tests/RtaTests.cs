namespace RasperryPI.RTAService.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RasperryBI.RTADataService.Data;

    [TestClass]
    public class RtaTests
    {
        /// <summary>
        /// Get Vehicle Test
        /// </summary>
        [TestMethod]
        public void GetVehicle()
        {
            var repo = new RTARepository(new RTAContext());
            Assert.IsNotNull(repo.GetRTA("Ford123"));
        }
    }
}
