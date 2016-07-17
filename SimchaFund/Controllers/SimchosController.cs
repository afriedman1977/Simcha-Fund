using SimchaFund.data;
using SimchaFund.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimchaFund.Controllers
{
    public class SimchosController : Controller
    {
        //
        // GET: /Simchos/

        public ActionResult AllSimchos()
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            AllSimchosViewModel asvm = new AllSimchosViewModel();
            foreach (Simcha s in manager.GetAllSimchos())
            {
                asvm.AllSimchos.Add(new SimchaViewModel
                {
                    Simcha = s,
                    AmountOfContributorsForSimcha = manager.CountOfContributionsForSimcha(s.Id),
                    TotalContributionsForSimcha = manager.SumOfContributionsForSimcha(s.Id)
                });
            }
            asvm.AmountOfContributors = manager.CountOfContributors();
            return View(asvm);
        }

        public ActionResult AddSimcha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitAddSimcha(string simchaName, DateTime date)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            manager.AddSimcha(simchaName, date);
            return Redirect("/Simchos/AllSimchos");
        }

        public ActionResult Contributions(int id)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            AllContributorsViewModel acvm = new AllContributorsViewModel();
            foreach (Contributor c in manager.GetallContributors())
            {                
                ContributorViewModel cvm = new ContributorViewModel();
                cvm.Contributor = c;
                cvm.Balance = manager.SumOfDeposits(c.Id) - manager.SumOfContributions(c.Id);                
                foreach (ContributionForSimcha cfs in manager.NamesOfContributorsForSimcha(id))
                {
                    if (cfs.ContributorId == c.Id)
                    {
                        cvm.Deposited = true;
                        cvm.Amount = cfs.Amount;
                    }
                }
                acvm.AllContributors.Add(cvm);
                //acvm.AllContributors.Add(new ContributorViewModel
                //{
                //    Contributor = c,
                //    Balance = manager.SumOfDeposits(c.Id) - manager.SumOfContributions(c.Id)

                //});
                //IEnumerable<ContributionForSimcha> cfs = manager.NamesOfContributorsForSimcha(id);
                //if(manager.NamesOfContributorsForSimcha(id).Any(x => x.ContributorId == cvm.Contributor.Id))
                //{
                //    cvm.Deposited = true;
                //}
                
            }
            acvm.CurrentSimcha = manager.GetSimchaById(id);
            return View(acvm);
        }

        [HttpPost]
        public ActionResult SubmitContributions(List<ContributionForSimcha> contributors)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            manager.AddContributionsForSimcha(contributors);
            return Redirect("/Simchos/AllSimchos");
        }

        public ActionResult ContributorsForSimcha(int id)
        {
            SimchaFundManager manager = new SimchaFundManager(Properties.Settings.Default.constr);
            SimchaViewModel svm = new SimchaViewModel();
            svm.Simcha = manager.GetSimchaById(id);
            svm.NamesOfContributorsForSimcha = manager.NamesOfContributorsForSimcha(id);
            return View(svm);
        }
    }
}
