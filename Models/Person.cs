using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class Person
    {
        private string phone;
        public Person()
        {
            UserInfos = new HashSet<UserInfo>();
            VaccinationBookings = new HashSet<VaccinationBooking>();
            VaccinationRecords = new HashSet<VaccinationRecord>();
            VaccinationTracks = new HashSet<VaccinationTrack>();
            VaccinationWanteds = new HashSet<VaccinationWanted>();
        }

        public int PersonId { get; set; }
        [DisplayName("身分證字號")]
        public string PersonIdentityId { get; set; }
        [DisplayName("健保卡卡號")]
        public string PersonHealthId { get; set; }
        public string PersonPassword { get; set; }
        public string PersonPasswordSalt { get; set; }
        public int? CountyPostalCode { get; set; }
        [DisplayName("居住地址")]
        public string PersonAdress { get; set; }
        [DisplayName("姓名")]
        public string PersonName { get; set; }
        [DisplayName("出生日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime? PersonBirthday { get; set; }
        [DisplayName("手機號碼")]
        public string PersonCellphoneNumber { get { return phone.Trim(); } set { phone = value.Trim(); } }
        [DisplayName("電子信箱")]
        public string PersonMail { get; set; }
        [DisplayName("職業")]
        public string PersonJob { get; set; }
        [DisplayName("性別")]
        public string PersonSex { get; set; }
        public byte[] PersonIdphoto { get; set; }

        public virtual County CountyPostalCodeNavigation { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
        public virtual ICollection<VaccinationBooking> VaccinationBookings { get; set; }
        public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; }
        public virtual ICollection<VaccinationTrack> VaccinationTracks { get; set; }
        public virtual ICollection<VaccinationWanted> VaccinationWanteds { get; set; }
    }
}
