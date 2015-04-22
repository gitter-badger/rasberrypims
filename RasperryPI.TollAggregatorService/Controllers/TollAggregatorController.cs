using System;
using System.Web.Http;
using RasperryBI.TollAggregatorService.Entities;
using RasperryPI.TollAggregatorService.Data;

namespace RasperryPI.TollAggregatorService.Controllers
{
    public class TollAggregatorController : ApiController
    {
        private readonly TollAggregatorRepository _tollAggregatorRepository;

        public TollAggregatorController()
        {
            _tollAggregatorRepository = new TollAggregatorRepository(new TollAggregatorContext());
        }


        [Route("tolls/{searchText}")]
        [HttpGet]
        public IHttpActionResult GetTolls(string searchText)
        {
            try
            {
                var tolls = string.IsNullOrEmpty(searchText) || searchText == "all" ? _tollAggregatorRepository.GetTolls() : _tollAggregatorRepository.GetTolls(searchText);
                return Ok(tolls);

            }
            catch (Exception e)
            {
                return Ok(e.ToString());
            }
        }

        [Route("post")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Toll toll)
        {
            try
            {
                toll.TollLocation = "Gachibowli ORR";
                var result = _tollAggregatorRepository.AddToll(toll);
                return Ok(result);

            }
            catch (Exception e)
            {
                return Ok(e.ToString());
            }
        }

    }
}
