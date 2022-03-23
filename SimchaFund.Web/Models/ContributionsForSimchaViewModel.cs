using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Models
{
    public class ContributionsForSimchaViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public string Name { get; set; }
        public int SimchaId { get; set; }
    }
}
