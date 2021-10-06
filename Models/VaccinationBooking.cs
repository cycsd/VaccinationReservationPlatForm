using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccinationBooking
    {
        public int VaccinationBookingId { get; set; }
        public int? PersonId { get; set; }
        public int? HospitalId { get; set; }
        public int? VaccineId { get; set; }
        public DateTime? VbbookingDate { get; set; }
        public TimeSpan? VbbookingTime { get; set; }
        public string VbcheckRemark { get; set; }
        public DateTime? VbclickMoment { get; set; }
        public int? Vbnumber { get; set; }
        public DateTime? VbappointmentDate { get; set; }
        public string VbchargeRemark { get; set; }
        public int? VaccineSerialNumber { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Person Person { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
