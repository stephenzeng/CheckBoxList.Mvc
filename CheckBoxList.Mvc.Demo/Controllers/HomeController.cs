using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CheckBoxList.Mvc.Common;
using CheckBoxList.Mvc.Demo.Models;

namespace CheckBoxList.Mvc.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExampleCheckBoxList()
        {
            var investmentOptions = GetInvestmentOptions();
            return View(investmentOptions);
        }

        [HttpPost]
        public ActionResult ExampleCheckBoxList(IList<CheckBoxListItem> investmentOptions)
        {
            if (investmentOptions == null)
                throw new ArgumentNullException("investmentOptions");

            var selectedOptions = investmentOptions
                .Where(i => i.IsChecked).Select(i => i.Text);

            ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions);

            return View(investmentOptions);
        }

        public ActionResult ExampleCheckBoxListFor()
        {
            var viewModel = new InvestmentViewModel {InvestmentOptions = GetInvestmentOptions()};
            viewModel.InvestmentOptions.First().IsChecked = true;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ExampleCheckBoxListFor(InvestmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedOptions = viewModel.InvestmentOptions
                    .Where(i => i.IsChecked).Select(i => i.Text);

                ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions);
            }

            return View(viewModel);
        }

        public ActionResult ExampleEnumCheckBoxList()
        {
            var extremeSports = new List<ExtremeSport>();
            return View(extremeSports);
        }

        [HttpPost]
        public ActionResult ExampleEnumCheckBoxList(IList<ExtremeSport> extremeSports)
        {
            if (extremeSports == null)
                throw new ArgumentNullException("extremeSports");

            var selectedOptions = extremeSports.Select(e => e.GetEnumDisplayName());
            ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions);

            return View(extremeSports);
        }

        public ActionResult ExampleEnumCheckBoxListFor()
        {
            var viewModel = new ExtremeSportViewModel();
            viewModel.ExtremeSports.Add(ExtremeSport.RockClimbing);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ExampleEnumCheckBoxListFor(ExtremeSportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedOptions = viewModel.ExtremeSports.Select(e => e.GetEnumDisplayName());
                ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions);
            }

            return View(viewModel);
        }

        private IList<CheckBoxListItem> GetInvestmentOptions()
        {
            return new[]
            {
                "Australian shares",
                "Australian fixed interest",
                "Property",
                "Commodity",
                "Australian bond",
                "International shares"

            }
                .OrderBy(i => i)
                .Select(i => new CheckBoxListItem() {Text = i, Value = i})
                .ToList();
        } 
    }
}