using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CVManageViewModel
    {
        public int? VaccineExp { get; set; }
        public string DiseaseName { get; set; }
        public int VaccinePsiid { get; set; }
        public int? HospitalId { get; set; }
        public int? VaccineId { get; set; }
        public string VaccineName { get; set; }
        public int? VaccinePsiquantity { get; set; }
        public string VaccinePsistateRemark { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? VaccinePsirecordTime { get; set; }
        public DateTime? VaccinePsimfDate { get; set; }
        public DateTime? VaccinePsiexpDate { get; set; }
      
    }
}

