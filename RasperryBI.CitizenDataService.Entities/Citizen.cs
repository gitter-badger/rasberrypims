using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasperryBI.CitizenDataService.Entities
{

    public class Citizen
    {
        public int CitizenId { get; set; }

        public string CitizenUniqueId { get; set; }

        public string CitizenName { get; set; }
    }
}
