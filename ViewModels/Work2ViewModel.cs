using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Models
{
    public class Work2ViewModel
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string PersonIdentityId { get; set; }       
        public int? Vbnumber { get; set; }       
        public string VbcheckRemark { get; set; }
        public int? VaccineSerialNumber { get; set; }        
        public DateTime? VbbookingDate { get; set; }       
        public DateTime? VbappointmentDate { get; set; }
        public TimeSpan? VbbookingTime { get; set; }
    }
}
