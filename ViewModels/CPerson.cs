using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CPerson
    {
        
        public Person Person { get; set; }
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
        [Required(ErrorMessage = "手機號碼為必填欄位")]
        [StringLength(10)]
        public string PersonCellphoneNumber { get; set; }
        [DisplayName("電子信箱")]
        public string PersonMail { get; set; }
        [DisplayName("職業")]
        public string PersonJob { get; set; }
        [DisplayName("性別")]
        public string PersonSex { get; set; }
        public byte[] PersonIdphoto { get; set; }
    }
}
