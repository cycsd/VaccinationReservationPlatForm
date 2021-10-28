using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class County
    {
        public int CountyPostalCode { get; set; }
        public string CountyTownName { get; set; }
        public string CountyName { get; set; }
    }
}
