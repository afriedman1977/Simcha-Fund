using SimchaFund.data;
using SimchaFund.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimchaFund.Controllers
{
    public class ContributorsController : Controller
    {
        //
        // GET: /Contributors/

        public ActionResult AllContributors()
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);          
            AllContributorsViewModel acvm = new AllContributorsViewModel();          
            foreach(Contributor c in manager.GetallContributors())
            {
                acvm.AllContributors.Add(new ContributorViewModel{
                    Contributor = c,
                    Balance = manager.SumOfDeposits(c.Id) - manager.SumOfContributions(c.Id)
                });
            }
            acvm.TotalBalance = manager.SumOfDeposits(0) - manager.SumOfContributions(0);
            return View(acvm);
        } 

        public ActionResult AddContributor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitAddContributor(string name, string phone, DateTime date, int deposit,int? include)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            if(include == null)
            {
                include = 0;
            }
            int contributorId = manager.AddContributor(name, phone,include.Value);
            manager.AddDeposit(deposit, date, contributorId);
            return Redirect("/Contributors/AllContributors");
        }

        public ActionResult AddDeposit(int id)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            return View(manager.GetContributorById(id));
        }

        [HttpPost]
        public ActionResult SubmitAddDeposit(int amount, DateTime date, int contributorId)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            manager.AddDeposit(amount, date, contributorId);
            return Redirect("/Contributors/AllContributors");
        }

        public ActionResult EditContributor(int id)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            return View(manager.GetContributorById(id));
        }

        [HttpPost]
        public ActionResult SubmitEditContributor(int id, string name, string phone,int? include)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            if (include == null)
            {
                include = 0;
            }
            manager.EditContributor(id, name, phone,include.Value);
            return Redirect("/Contributors/Allcontributors");
        }

        public ActionResult History(int id)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            AllHistoryViewModel ahvm = new AllHistoryViewModel();
            foreach(Deposit d in manager.GetAllDeposits(id))
            {
                ahvm.AllHistory.Add(new HistoryViewModel
                {
                    Action = "Deposit",
                    Amount = d.Amount,
                    Date = d.DepositDate
                });
            }
            foreach(Contribution c in manager.GetAllContributions(id))
            {
                ahvm.AllHistory.Add(new HistoryViewModel
                {
                    Action = "Contribution to the " + c.SimchaName,
                    Amount = - c.Amount,
                    Date =c.Date
                });
            }
            ahvm.Contributor = manager.GetContributorById(id);
            ahvm.Balance = manager.SumOfDeposits(id) - manager.SumOfContributions(id);
            ahvm.AllHistory = ahvm.AllHistory.OrderBy(h => h.Date).ToList();
            return View(ahvm);
        }

    }
}
