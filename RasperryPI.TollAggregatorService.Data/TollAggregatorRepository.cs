using System.Collections.Generic;
using System.Linq;
using RasperryBI.TollAggregatorService.Entities;

namespace RasperryPI.TollAggregatorService.Data
{
    /// <summary>
    /// Rta Repository class
    /// </summary>
    public class TollAggregatorRepository
    {
        /// <summary>
        /// rta context variable
        /// </summary>
        private readonly TollAggregatorContext _tollAggregatorContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tollAggregatorContext">RTA Context</param>
        public TollAggregatorRepository(TollAggregatorContext tollAggregatorContext)
        {
            _tollAggregatorContext = tollAggregatorContext;
        }

        /// <summary>
        /// Get RTA
        /// </summary>
        /// <param name="tollId">rta Uniwue Id</param>
        /// <returns>RTA class object based on rtaUnique Id</returns>
        public Toll GetToll(int tollId)
        {
            return _tollAggregatorContext.TollList.FirstOrDefault(p => p.TollId == tollId);
        }

        public List<Toll> GetTolls(string searchText)
        {
            return _tollAggregatorContext.TollList.Where(p => p.VehicleModel.ToLower().Contains(searchText.ToLower())
                || p.CitizenName.ToLower().Contains(searchText.ToLower())).ToList();
        }

        public List<Toll> GetTolls()
        {
            return _tollAggregatorContext.TollList.ToList();
        }

        public bool AddToll(Toll toll)
        {

            _tollAggregatorContext.TollList.Add(toll);

            // call SaveChanges method to save student into database
            _tollAggregatorContext.SaveChanges();

            return true;
        }
    }
}
