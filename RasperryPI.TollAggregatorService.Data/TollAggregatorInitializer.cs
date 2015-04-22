using System.Collections.Generic;
using RasperryBI.TollAggregatorService.Entities;

namespace RasperryPI.TollAggregatorService.Data
{
    /// <summary>
    /// RTAInitializer class
    /// </summary>
    public class TollAggregatorInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TollAggregatorContext>
    {
        /// <summary>
        /// Seed Method
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(TollAggregatorContext context)
        {
            var students = new List<Toll>()
                {
                    new Toll(){TollId = 1,CitizenName = "Srini",VehicleModel = "A-Star"},
                    new Toll(){TollId = 2,CitizenName = "Srini2",VehicleModel = "A-Star-2"},
                };
            students.ForEach(s => context.TollList.Add(s));
            context.SaveChanges();
        }
    }
}