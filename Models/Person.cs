using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class Person
    {
        public Person()
        {
            UserInfos = new HashSet<UserInfo>();
            VaccinationBookings = new HashSet<VaccinationBooking>();
            VaccinationRecords = new HashSet<VaccinationRecord>();
            VaccinationTracks = new HashSet<VaccinationTrack>();
            VaccinationWanteds = new HashSet<VaccinationWanted>();
        }

        public int PersonId { get; set; }
        public string PersonIdentityId { get; set; }
        public string PersonHealthId { get; set; }
        public string PersonPassword { get; set; }
        public string PersonPasswordSalt { get; set; }
        public int? CountyPostalCode { get; set; }
        public string PersonAdress { get; set; }
        public string PersonName { get; set; }
        public DateTime? PersonBirthday { get; set; }
        public string PersonCellphoneNumber { get; set; }
        public string PersonMail { get; set; }
        public string PersonJob { get; set; }
        public string PersonSex { get; set; }
        public byte[] PersonIdphoto { get; set; }

        public virtual ICollection<UserInfo> UserInfos { get; set; }
        public virtual ICollection<VaccinationBooking> VaccinationBookings { get; set; }
        public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; }
        public virtual ICollection<VaccinationTrack> VaccinationTracks { get; set; }
        public virtual ICollection<VaccinationWanted> VaccinationWanteds { get; set; }
    }
}
