using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class County
    {
        public County()
        {
            Hospitals = new HashSet<Hospital>();
            People = new HashSet<Person>();
        }

        public int CountyPostalCode { get; set; }
        public string CountyTownName { get; set; }
        public string CountyName { get; set; }

        public virtual ICollection<Hospital> Hospitals { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
