using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RasperryBI.CitizenDataService.Data;
using RasperryBI.CitizenDataService.Entities;

namespace RasperryPI.CitizenDataService.Controllers
{
    public class CitizenController : ApiController
    {
        private readonly CitizenRepository _citizenRepository;

        public CitizenController()
        {
            _citizenRepository = new CitizenRepository(new CitizenContext());
        }

        public CitizenController(CitizenRepository citizenRepository)
        {
            _citizenRepository = citizenRepository;
        }

        [Route("citizens/{citizenUniqueId}")]
        public IHttpActionResult GetCitizen(string citizenUniqueId)
        {
            var product = _citizenRepository.GetCitizen(citizenUniqueId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
