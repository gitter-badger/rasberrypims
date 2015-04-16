using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RasperryPI.RTAService.Controllers
{
    public class RtaController : ApiController
    {
        [Route("rta/{vehicleno}")]
        public IHttpActionResult GetVehicleNo(string vehicleno)
        {
          throw new NotImplementedException();
        }
    }
}
