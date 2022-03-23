using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimchaFund.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SimchaFund.Data;

namespace SimchaFund.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            var mgr = new Manager(_connectionString);
            string message = (string)TempData["Message"];
            SimchasViewModel vm = new SimchasViewModel();
            vm.Message = message;
         int Amount=   mgr.GetContributorAmount();
            vm.Simchas = mgr.GetSimchas(Amount);
            vm.ContributorAmount = Amount;
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddSimcha(string name, DateTime date)
        {
            var mgr = new Manager(_connectionString);
            int id=mgr.AddSimcha(name, date);
            TempData["Message"] = $"New Simcha Created ID:{id}";
            return Redirect("/");
        }
         public IActionResult Contributions(int simchaid)
        {
            var mgr = new Manager(_connectionString);
            List<Contributor> contributors = mgr.GetContributors();
            foreach(Contributor c in contributors)
            {
               decimal deposits= mgr.GetDeposit(c.Id);
               
                decimal withdrawals = mgr.Getwithdrawals(c.Id);
                decimal balance = deposits - withdrawals;
                c.Balance = balance;
                c.Amount = mgr.GetAmount(c.Id, simchaid);
                if(c.Amount==0)
                {
                    c.Amount = 5;
                }
                else
                {
                    c.Include = true;
                }

            }
            var vm = new ContributionsForSimchaViewModel();
            vm.Contributors = contributors;
            vm.SimchaId = simchaid;
           //s string table = "Simchas";
            vm.Name = mgr.GetSimchaName(simchaid);
            return View(vm);
        }

        public IActionResult UpdateContributions( List <Contributor> contributors, int simchaid)
        {
            var mgr = new Manager(_connectionString);
            mgr.DeleteForSimcha(simchaid);
            foreach (Contributor c in contributors)
            {
                if (c.AlwaysInclude || c.Include)
                {
                    Contribution add = new Contribution
                    {
                        PersonId = c.Id,
                        SimchaId = simchaid,
                        Amount = c.Amount
                    };
                    mgr.AddContribution(add);
                }
            }
            TempData["Message"] = $"Simcha {simchaid} Updated succesfully!!!";
            return Redirect("/");

        }



    }
}
