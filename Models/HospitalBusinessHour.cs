using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class HospitalBusinessHour
    {
        public int HospitalBusinessHourId { get; set; }
        public int? HospitalBusinessDayId { get; set; }
        public TimeSpan? HbhstartTime { get; set; }
        public TimeSpan? HbhendTime { get; set; }

        public virtual HospitalBusinessDay HospitalBusinessDay { get; set; }
    }
}
