using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class Vaccine
    {
        public Vaccine()
        {
            VaccinationBookings = new HashSet<VaccinationBooking>();
            VaccinationRecords = new HashSet<VaccinationRecord>();
            VaccinationTracks = new HashSet<VaccinationTrack>();
            VaccinationWanteds = new HashSet<VaccinationWanted>();
            VaccinePsis = new HashSet<VaccinePsi>();
            VaccineStocks = new HashSet<VaccineStock>();
            VaccineToDiseases = new HashSet<VaccineToDisease>();
        }

        public int VaccineId { get; set; }
        public string VaccineName { get; set; }
        public string VaccineOfficialName { get; set; }
        public int? VaccineDistinctTime { get; set; }
        public int? VaccineExp { get; set; }
        public string VaccineStoredInfo { get; set; }
        public int? VaccineDoses { get; set; }

        public virtual ICollection<VaccinationBooking> VaccinationBookings { get; set; }
        public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; }
        public virtual ICollection<VaccinationTrack> VaccinationTracks { get; set; }
        public virtual ICollection<VaccinationWanted> VaccinationWanteds { get; set; }
        public virtual ICollection<VaccinePsi> VaccinePsis { get; set; }
        public virtual ICollection<VaccineStock> VaccineStocks { get; set; }
        public virtual ICollection<VaccineToDisease> VaccineToDiseases { get; set; }
    }
}
