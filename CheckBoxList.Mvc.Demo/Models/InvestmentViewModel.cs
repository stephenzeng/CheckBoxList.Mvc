using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckBoxList.Mvc.Demo.Models
{
    public class InvestmentViewModel
    {
        public InvestmentViewModel()
        {
            InvestmentOptions = new[]
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

        [Display(Name = "What investment options would you be interested in?")]
        public IList<CheckBoxListItem> InvestmentOptions { get; set; }
    }
}