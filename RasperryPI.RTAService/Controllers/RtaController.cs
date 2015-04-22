namespace RasperryPI.RTAService.Controllers
{
    using System.Web.Http;
    using RasperryBI.RTADataService.Data;

    /// <summary>
    /// RTA Controller class
    /// </summary>
    public class RtaController : ApiController
    {
        /// <summary>
        /// RTARepository variable
        /// </summary>
        private readonly RTARepository _rtaRepository;

        /// <summary>
        /// RTAController Constructor
        /// </summary>
        public RtaController()
        {
            _rtaRepository = new RTARepository(new RTAContext());
        }

        /// <summary>
        /// RTAController Constructor
        /// </summary>
        /// <param name="rtaRepository"></param>
        public RtaController(RTARepository rtaRepository)
        {
            _rtaRepository = rtaRepository;
        }

        /// <summary>
        /// GetVehicleNo Method
        /// </summary>
        /// <param name="vehicleno">Accepts Vehicle No</param>
        /// <returns>returns RTA JSON</returns>
        [Route("rta/{vehicleno}")]
        public IHttpActionResult GetVehicleNo(string vehicleno)
        {
            var product = _rtaRepository.GetRTA(vehicleno);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
