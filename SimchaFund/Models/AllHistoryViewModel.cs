using SimchaFund.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaFund.Models
{
    public class AllHistoryViewModel
    {
        public List<HistoryViewModel> AllHistory { get; set; }
        public Contributor Contributor { get; set; }
        public decimal Balance { get; set; }

        public AllHistoryViewModel()
        {
            AllHistory = new List<HistoryViewModel>();
        }
    }
}