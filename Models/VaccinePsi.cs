using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccinePsi
    {
        public int VaccinePsiid { get; set; }
        public int? HospitalId { get; set; }
        public int? VaccineId { get; set; }
        public int? VaccinePsiquantity { get; set; }
        public string VaccinePsistateRemark { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? VaccinePsirecordTime { get; set; }
        public DateTime? VaccinePsimfDate { get; set; }
        public DateTime? VaccinePsiexpDate { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
