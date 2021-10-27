using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModel
{
    public class VaccinationRecordTrackViewModel
    {
        public int VaccinationTrackId { get; set; }
        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public int VaccineId { get; set; }
        public string VaccineName { get; set; }
        public string HospitalName { get; set; }

        public int VTtimes { get; set; }
        public string VRgivenDate { get; set; }

        public DateTime VTappointmentDate { get; set; }
        public string DiseaseName { get; internal set; }
    }
}
