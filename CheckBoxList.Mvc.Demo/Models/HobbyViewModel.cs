using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CheckBoxList.Mvc.Demo.Models
{
    public class HobbyViewModel
    {
        [Display(Name = "What hobbies do you have in your spare time?")]
        public IList<CheckBoxListItem> Hobbies { get; set; }

        public IList<Hobby> MyHobbies { get; set; } 
    }

    public enum Hobby
    {
        Reading = 0,
        Painting = 1,
        Dance = 3,
        Singing = 4,
        Photography = 5,
        Gambling = 6,
        Writing = 7,
        Cooking = 8,
        
        [Description("Hiking or camping")]
        HikingCamping = 9,

        [Description("Scuba diving")]
        ScubaDiving = 10,

        Swimming = 11,

        [Description("Rock climbing")]
        RockClimbing = 12,

        Other = 13,
    }
}