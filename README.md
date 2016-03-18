# CheckBoxList.Mvc
#####Extension methods for CheckBoxList in MVC

####Overview

CheckBoxList.Mvc aims to help you to code for mulitple selctions in ASP.NET MVC in the, well, MVC way. For example in the view `@Html.TextBoxFor(m => m.Name)` gives you a textbox input and other out-of-box benefits from ASP.NET MVC. With CheckBoxList.Mvc you now can do the similar thing: `@Html.CheckBoxListFor(m => m.Options)`, where `m.Options` is a type of `IList<CheckBoxListItem>`.

####How to use it

ViewModel
```c#
public class InvestmentViewModel
{
    [Display(Name = "What investment options would you be interested in?")]
    public IList<CheckBoxListItem> InvestmentOptions { get; set; }
}
```

Controller actions
```c#
public ActionResult Example1()
{
    var viewModel = new InvestmentViewModel
    {
        //can use AutoMapper for the mappings between entity and CheckBoxListItem
        //can also set CheckBoxListItem.IsChecked to true for the pre-selected options
        InvestmentOptions = GetInvestmentOptions()
    };

    return View(viewModel);
}

[HttpPost]
public ActionResult Example1(InvestmentViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        var selectedOptions = viewModel.InvestmentOptions.Where(i => i.IsChecked);

        //can use AutoMapper for the mappings between entity and CheckBoxListItem
        return SaveInvestmentOptions(selectedOptions);
    }

    return View(viewModel);
}
```

View
```html
<div class="form-group">
    @Html.LabelFor(m => m.InvestmentOptions)
    <div>
        @Html.CheckBoxListFor(m => m.InvestmentOptions)
    </div>
</div>
```

####Support for Enum

It can be convenient to use a list of enum types in your enity, since the type itself already has all the available options, so that in the entity list it only needs to store the ones selected. 

It can be done by using the extension methods `ToCheckboxListItems<TEnum>()` and `ToEnumArray<TEnum>`.

Enum
```c#
public enum Hobby
{
    [Description("Hiking or camping")]
    HikingCamping = 0,

    [Description("Scuba diving")]
    ScubaDiving = 1,

    Swimming = 2,

    [Description("Rock climbing")]
    RockClimbing = 3,

    Other = 4,
}
```

ViewModel
```c#
public class HobbyViewModel
{
    [Display(Name = "What hobbies do you have in your spare time?")]
    public IList<CheckBoxListItem> Hobbies { get; set; }
}
```

Controller actions
```#
public ActionResult Example2()
{
    IList<Hobby> existingHobbies = GetHobbies();

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
        var selectedHobbies = viewModel.Hobbies.ToEnumArray<Hobby>();
        return SaveHobbies(selectedHobbies);
    }

    return View(viewModel);
}
```

The view is the same.

