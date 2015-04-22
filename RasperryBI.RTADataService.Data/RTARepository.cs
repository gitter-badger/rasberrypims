namespace RasperryBI.RTADataService.Data
{
    using System.Linq;
    using Entities;

    /// <summary>
    /// Rta Repository class
    /// </summary>
    public class RTARepository
    {
        /// <summary>
        /// rta context variable
        /// </summary>
        private readonly RTAContext _rtaContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rtaContext">RTA Context</param>
        public RTARepository(RTAContext rtaContext)
        {
            _rtaContext = rtaContext;
        }

        /// <summary>
        /// Get RTA
        /// </summary>
        /// <param name="rtaUniqueId">rta Uniwue Id</param>
        /// <returns>RTA class object based on rtaUnique Id</returns>
        public RTA GetRTA(string rtaUniqueId)
        {
            return _rtaContext.RTAList.FirstOrDefault(p => p.Vehicleno == rtaUniqueId);
        }
    }
}
