using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccinationWanted
    {
        public int VaccinationWantedId { get; set; }
        public int? PersonId { get; set; }
        public int? VaccineId { get; set; }
        public string VwleftoverWillingness { get; set; }
        public string VwassignMark { get; set; }

        public virtual Person Person { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
