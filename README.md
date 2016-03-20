CheckBoxList.Mvc
================

#####Extension methods for CheckBoxList in MVC

Overview
--------

CheckBoxList.Mvc aims to help you to code for mulitple selctions in ASP.NET MVC in the, well, MVC way. For example in the view `@Html.TextBoxFor(m => m.Name)` gives you a textbox input and other out-of-box benefits from ASP.NET MVC. With CheckBoxList.Mvc you now can do the similar thing with the following methods: 

* `@Html.CheckBoxList("name", list)` 
* `@Html.CheckBoxListFor(m => m.List)` 
 
`list` and `m.List` are type of `IList<CheckBoxListItem>`

* `@Html.EnumCheckBoxList("name", list)`
* `@Html.EnumCheckBoxListFor(m => m.List)` 
 
`list` and `m.List`are type of `IList<TEnum>`

Nuget package
-------------

You can search CheckBoxList.Mvc in Nuget Package Manager, or run the following command in the Package Manager Console to install it.

```
PM> Install-Package CheckBoxList.Mvc
```

How to use it
-------------

The demo project contains 4 simple examples of using the 4 extension methods which should be able to help you to start with.

Support for Enum list
---------------------

It can be quite handy to use a list of enum types in your models for multiple choices, since the type itself already has all the available options, so that what contained in the model list is what options selected. `EnumCheckBoxList()` and `EnumCheckBoxListFor()` are for this purpose.

######Enum
```c#
public enum ExtremeSport
{
    [Description("Bungee jumping")]
    BungeeJumping = 0,

    [Description("Deep diving")]
    DeepDiving = 1,

    Kitesurfing = 2,

    Parachute = 3,
}
```

######ViewModel
```c#
public class ExtremeSportViewModel
{
    public ExtremeSportViewModel()
    {
        ExtremeSports = Enumerable.Empty<ExtremeSport>().ToList();
    }

    [Display(Name = "Do you do any of the extreme sports listed below in your spare time?")]
    public IList<ExtremeSport> ExtremeSports { get; set; } 
}
```

######Controller actions
```#
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
```

######View
```
@using (Html.BeginForm())
{
    <div class="form-group">
        @Html.LabelFor(m => m.ExtremeSports)
        @Html.EnumCheckBoxListFor(m => m.ExtremeSports)
    </div>
    <div>
        <button type="submit" class="btn btn-default">Submit</button>
    </div>
}
```

