using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CHospitalUser
    {
        public int hospitalUserId { get; set; }
        public string hospitalUserName { get; set; }
        public string hospitalUserPassword { get; set; }
        public string hospitalUserPasswordSalt { get; set; }
    }
}
