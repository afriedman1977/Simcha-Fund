using SimchaFund.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaFund.Models
{
    public class ContributorViewModel
    {
        public Contributor Contributor { get; set; }
        public decimal Balance { get; set; }
        public bool Deposited { get; set; }
        public decimal Amount { get; set; }
    }
}