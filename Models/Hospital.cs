using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class Hospital
    {
        public Hospital()
        {
            HospitalBusinessDays = new HashSet<HospitalBusinessDay>();
            UserForHospitals = new HashSet<UserForHospital>();
            VaccinationBookings = new HashSet<VaccinationBooking>();
            VaccinationRecords = new HashSet<VaccinationRecord>();
            VaccinePsis = new HashSet<VaccinePsi>();
            VaccineStocks = new HashSet<VaccineStock>();
        }

        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public int? CountyPostalCode { get; set; }
        public string HospitalAdress { get; set; }
        public string HospitalPhone { get; set; }
        public string HospitalMail { get; set; }
        public int? HospitalCapacity { get; set; }
        public int? HospitalClass { get; set; }
        public byte[] HospitalPhoto { get; set; }
        public string HospitalAgencyCode { get; set; }
        public string HospitalContType { get; set; }
        public string HospitalType { get; set; }
        public string HospitalCategory { get; set; }
        public int? HospitalPhoneAreaCode { get; set; }

        public virtual County CountyPostalCodeNavigation { get; set; }
        public virtual ICollection<HospitalBusinessDay> HospitalBusinessDays { get; set; }
        public virtual ICollection<UserForHospital> UserForHospitals { get; set; }
        public virtual ICollection<VaccinationBooking> VaccinationBookings { get; set; }
        public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; }
        public virtual ICollection<VaccinePsi> VaccinePsis { get; set; }
        public virtual ICollection<VaccineStock> VaccineStocks { get; set; }
    }
}
