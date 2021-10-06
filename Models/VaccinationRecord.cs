using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccinationRecord
    {
        public int VaccinationRecordId { get; set; }
        public int? PersonId { get; set; }
        public int? HospitalId { get; set; }
        public int? VaccineId { get; set; }
        public DateTime? VrgivenDate { get; set; }
        public int? VaccineSerialNumber { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Person Person { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
