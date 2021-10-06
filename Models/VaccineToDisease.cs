using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccineToDisease
    {
        public int VaccineToDiseaseId { get; set; }
        public int? VaccineId { get; set; }
        public int? DiseaseId { get; set; }
        public int? VtdrequiredNumber { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
