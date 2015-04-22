    using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RasperryBI.RTADataService.Entities;

namespace RasperryBI.RTADataService.Data
{
    /// <summary>
    /// RTA Context Class
    /// </summary>
    public class RTAContext : DbContext
    {
        /// <summary>
        /// RTA class Constructor
        /// </summary>
        public RTAContext()
            : base("RTAContext")
        {
        }

        /// <summary>
        /// RTA List property
        /// </summary>
        public DbSet<RTA> RTAList { get; set; }

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