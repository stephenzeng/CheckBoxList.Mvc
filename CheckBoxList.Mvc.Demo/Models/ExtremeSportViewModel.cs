using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckBoxList.Mvc.Demo.Models
{
    public class ExtremeSportViewModel
    {
        public ExtremeSportViewModel()
        {
            ExtremeSports = Enumerable.Empty<ExtremeSport>().ToList();
        }

        [Display(Name = "Do you do any of the extreme sports listed below in your spare time?")]
        public IList<ExtremeSport> ExtremeSports { get; set; } 
    }

    public enum ExtremeSport
    {
        [Description("Bungee jumping")]
        BungeeJumping = 0,

        [Description("Deep diving")]
        DeepDiving = 1,

        Kitesurfing = 2,

        Parachute = 3,

        [Description("Rock climbing")]
        RockClimbing = 4,

        [Description("Wingsuit sky diving")]
        WingsuitSkyDiving = 5,
    }
}