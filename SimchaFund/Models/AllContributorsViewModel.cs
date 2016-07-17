using SimchaFund.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaFund.Models
{
    public class AllContributorsViewModel
    {
        public List<ContributorViewModel> AllContributors { get; set; }
        public decimal TotalBalance { get; set; }
        public Simcha CurrentSimcha { get; set; }

        public AllContributorsViewModel()
        {
            AllContributors = new List<ContributorViewModel>();
        }
    }
}