using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CWantedVaccine
    {
        public int? PersonId { get; set; }
        public int? VaccineId { get; set; }
        public string VaccineName { get; set; }

    }
}
