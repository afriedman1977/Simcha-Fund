using SimchaFund.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaFund.Models
{
    public class SimchaViewModel
    {
        public Simcha Simcha { get; set; }
        public int AmountOfContributorsForSimcha { get; set; }
        public decimal TotalContributionsForSimcha { get; set; }
        public IEnumerable<ContributionForSimcha> NamesOfContributorsForSimcha { get; set; }
    }
}