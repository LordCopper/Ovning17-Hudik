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
        string Name { get; set; }
        DateTime StartTime { get; set; }
        TimeSpan Duration { get; set; }
        DateTime EndTime { get { return StartTime + Duration; } }
        public  string Description { get; set; }
        public virtual ICollection<ApplicationUser> AttendingMembers { get; set; }
    }
}