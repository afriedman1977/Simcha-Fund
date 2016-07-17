using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaFund.Models
{
    public class HistoryViewModel
    {
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}