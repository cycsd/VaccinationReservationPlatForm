using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class HospitalUser
    {
        public HospitalUser()
        {
            UserForHospitals = new HashSet<UserForHospital>();
        }

        public int HospitalUserId { get; set; }
        public string HospitalUserName { get; set; }
        public string HospitalUserPassword { get; set; }
        public string HospitalUserPasswordSalt { get; set; }

        public virtual ICollection<UserForHospital> UserForHospitals { get; set; }
    }
}
