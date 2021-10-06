using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccinationTrack
    {
        public int VaccinationTrackId { get; set; }
        public int? PersonId { get; set; }
        public int? VaccineId { get; set; }
        public int? Vttimes { get; set; }
        public DateTime? VtappointmentDate { get; set; }

        public virtual Person Person { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
