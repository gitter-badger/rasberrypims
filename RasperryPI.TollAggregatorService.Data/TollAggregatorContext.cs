using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RasperryBI.TollAggregatorService.Entities;

namespace RasperryPI.TollAggregatorService.Data
{
    /// <summary>
    /// RTA Context Class
    /// </summary>
    public class TollAggregatorContext : DbContext
    {
        /// <summary>
        /// RTA class Constructor
        /// </summary>
        public TollAggregatorContext()
            : base("TollAggregatorContext")
        {
        }

        /// <summary>
        /// RTA List property
        /// </summary>
        public DbSet<Toll> TollList { get; set; }

        /// <summary>
        /// OnModel Creating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}