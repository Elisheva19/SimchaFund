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
    public class Contributors : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            var mgr = new Manager(_connectionString);
            var vm = new ContributorsViewModel();
            List<Contributor> contributors = mgr.GetContributors();
            foreach (Contributor c in contributors)
            {
                decimal deposits = mgr.GetDeposit(c.Id);

                decimal withdrawals = mgr.Getwithdrawals(c.Id);
                decimal balance = deposits - withdrawals;
                c.Balance = balance;


            }
            vm.Contributors = contributors;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Deposit(Deposit newdeposit)
        {
            var mgr = new Manager(_connectionString);
            mgr.AddDeposit(newdeposit);
            return Redirect("/contributors/index");
        }

        public IActionResult NewContributor(Contributor newcontri)
        {
            var mgr = new Manager(_connectionString);
           int newid= mgr.AddContributor(newcontri);
           var initialdeposit = new Deposit{
               PersonId= newid,
               Amount=newcontri.Amount,
               Date=  newcontri.DateCreated
            };
            mgr.AddDeposit(initialdeposit);
            return Redirect("/contributors/index");
        }

        public IActionResult UpdateContributor(Contributor update)
        {
            var mgr = new Manager(_connectionString);
            mgr.UpdateContributor(update);
            return Redirect("/contributors/index");

        }

        public IActionResult History(int contributorid)
        {
            var mgr = new Manager(_connectionString);
            var vm = new HistoryViewModel();
            decimal deposits = mgr.GetDeposit(contributorid);

            decimal withdrawals = mgr.Getwithdrawals(contributorid);
            decimal balance = deposits - withdrawals;
            vm.Balance = balance;
           vm.ContributorName = mgr.GetContributorName( contributorid);
            List<HistoryTrans> Contr = mgr.History(contributorid);
            vm.Transactions = mgr.GetHistoryDeposits(contributorid, Contr).OrderByDescending(t => t.Date).ToList();
            return View(vm);
        }
    }
}
