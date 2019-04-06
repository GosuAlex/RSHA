using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models
{
    public class Requests
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public int PhoneNumber { get; set; }

        [Required]
        public string Location { get; set; }

        [Display(Name = "Car license plate")]
        public int CarLicensePlate { get; set; }

        [Display(Name = "Car model")]
        public string CarModel { get; set; }

        public int ProblemId { get; set; }
        [ForeignKey("ProblemId")]
        public virtual ProblemTypes ProblemTypes { get; set; }

        public string Message { get; set; }

        public DateTime RequestCreated { get; set; }

        public DateTime RequestScheduledDate { get; set; }

        public DateTime RequestScheduledTime { get; set; }

        public bool Completed { get; set; } = true;

        public string MechanicAssigned { get; set; }
    }
}
