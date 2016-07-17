using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaFund.Models
{
    public class AllSimchosViewModel
    {
        public List<SimchaViewModel> AllSimchos { get; set; }
        public int AmountOfContributors { get; set; }
        
        public AllSimchosViewModel()
        {
            AllSimchos = new List<SimchaViewModel>();
        }
    }
}