using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class UserForHospital
    {
        public int UserForHospitalId { get; set; }
        public int? HospitalUserId { get; set; }
        public int? HospitalId { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual HospitalUser HospitalUser { get; set; }
    }
}
