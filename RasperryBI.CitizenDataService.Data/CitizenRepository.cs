using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RasperryBI.CitizenDataService.Entities;

namespace RasperryBI.CitizenDataService.Data
{
    public class CitizenRepository
    {
        private readonly CitizenContext _citizenContext;

        public CitizenRepository(CitizenContext citizenContext)
        {
            _citizenContext = citizenContext;
        }

        public Citizen GetCitizen(string citizenUniqueId)
        {
            return _citizenContext.Citizens.FirstOrDefault(p => p.CitizenUniqueId == citizenUniqueId);
        }
    }

    public class CitizenContext : DbContext
    {

        public CitizenContext()
            : base("CitizenContext")
        {
        }

        public DbSet<Citizen> Citizens { get; set; }
     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class CitizenInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CitizenContext>
    {
        protected override void Seed(CitizenContext context)
        {
            var students = new List<Citizen>
            {
            new Citizen{CitizenName= "Srini",CitizenUniqueId= "Srini"}
            };

            students.ForEach(s => context.Citizens.Add(s));
            context.SaveChanges();
            
        }
    }
}
