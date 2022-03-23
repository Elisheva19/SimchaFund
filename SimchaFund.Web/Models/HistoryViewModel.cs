using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimchaFund.Web.Models
{
    public class HistoryViewModel
    {
        public decimal Balance { get; set; }
        public string ContributorName { get; set; }
        public List<HistoryTrans> Transactions { get; set; }
    }
}
