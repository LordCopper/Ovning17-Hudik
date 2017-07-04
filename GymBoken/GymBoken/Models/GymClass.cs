using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymBoken.Models
{
	public class GymClass
	{
        public int Id { get; set; }
        [Display(Name = "Passnamn")]
        public string Name { get; set; }
        [Display(Name = "Startar")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Tidsåtgång")]
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> AttendingMembers { get; set; }
    }
}