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

        public ActionResult Example1()
        {
            var viewModel = new InvestmentViewModel();
            viewModel.InvestmentOptions.Last().IsChecked = true;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Example1(InvestmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedOptions = viewModel.InvestmentOptions
                    .Where(i => i.IsChecked).Select(i => i.Text);

                ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions);
            }

            return View(viewModel);
        }

        public ActionResult Example2()
        {
            var existingHobbies = new[] {Hobby.ScubaDiving, Hobby.Swimming,};

            var viewModel = new HobbyViewModel()
            {
                Hobbies = existingHobbies.ToCheckboxListItems().ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Example2(HobbyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedOptions = viewModel.Hobbies.ToEnumArray<Hobby>();

                ViewBag.SelectedOptionsText = string.Join(", ", selectedOptions.Select(i => i.GetEnumDisplayName()));
            }

            return View(viewModel);
        }
    }
}