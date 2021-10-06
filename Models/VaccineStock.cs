using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccineStock
    {
        public int VaccineStockId { get; set; }
        public int? HospitalId { get; set; }
        public int? VaccineId { get; set; }
        public int? VaccineStockUnit { get; set; }
        public int? VaccineStockConsumption { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }
}
