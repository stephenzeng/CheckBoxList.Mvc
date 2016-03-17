using System.Linq;
using System.Web.Mvc;
using CheckBoxList.Mvc.Demo.Models;

namespace CheckBoxList.Mvc.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new InvestmentViewModel();
            //viewModel.InvestmentOptions[0]

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(InvestmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedOptions = viewModel.InvestmentOptions
                    .Where(i => i.IsChecked).Select(i => i.Text);

                ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions);
            }

            return View(viewModel);
        }
    }
}