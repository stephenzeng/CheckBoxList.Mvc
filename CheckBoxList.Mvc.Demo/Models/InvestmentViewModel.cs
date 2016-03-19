using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckBoxList.Mvc.Demo.Models
{
    public class InvestmentViewModel
    {
        [Display(Name = "What investment options would you be interested in?")]
        public IList<CheckBoxListItem> InvestmentOptions { get; set; }
    }
}