using System.Collections.Generic;
using RasperryBI.RTADataService.Entities;

namespace RasperryBI.RTADataService.Data
{
    /// <summary>
    /// RTAInitializer class
    /// </summary>
    public class RTAInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<RTAContext>
    {
        /// <summary>
        /// Seed Method
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(RTAContext context)
        {
            var students = new List<RTA>
            {
                new RTA {Model = "Ford", OwnerUniqueId= "Ford123",Name = "Ford_User1",Registrationlocation = "Hyderabad",Vehicleno = "5555"}
            };

            students.ForEach(s => context.RTAList.Add(s));
            context.SaveChanges();
        }
    }
}