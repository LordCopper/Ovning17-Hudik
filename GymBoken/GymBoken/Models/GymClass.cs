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
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Display(Name = "Starttid")]
		public DateTime StartTime { get; set; }
		[Display(Name = "Passlängd")]
		public TimeSpan Duration { get; set; }
		[Display(Name = "Sluttid")]
		public DateTime EndTime { get { return StartTime + Duration; } }
		[Display(Name = "Beskrivning")]
		public string Description { get; set; }

		[Display(Name = "Deltagare")]
		public virtual ICollection<ApplicationUser> AttendingMembers { get; set; }
	}
}