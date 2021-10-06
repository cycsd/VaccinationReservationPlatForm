using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class HospitalBusinessDay
    {
        public HospitalBusinessDay()
        {
            HospitalBusinessHours = new HashSet<HospitalBusinessHour>();
        }

        public int HospitalBusinessDayId { get; set; }
        public int? HospitalId { get; set; }
        public int? Hbdweekday { get; set; }
        public int? Hbdmark { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual ICollection<HospitalBusinessHour> HospitalBusinessHours { get; set; }
    }
}
