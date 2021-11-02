using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CNumberInquireViewModel
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public int? CountyPostalCode { get; set; }
        public string HospitalAdress { get; set; }
    }
}
