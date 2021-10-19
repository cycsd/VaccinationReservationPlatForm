using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CUserInfoModel
    {
        [DisplayName("身分證字號")]
        public string labPersonIdentityID { get; set; }
        [DisplayName("健保卡卡號")]
        public string labPersonHealthID { get; set; }
        [DisplayName("郵遞區號")]
        public string labCountyPostalCode { get; set; }
        [DisplayName("居住地址")]
        public string txtPersonAdress { get; set; }
        [DisplayName("姓名")]
        public string labPersonName { get; set; }
        [DisplayName("出生日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime labPersonBirthday { get; set; }
        [DisplayName("手機號碼")]
        public string txtPersonCellphoneNumber { get; set; }
        [DisplayName("電子信箱")]
        public string txtPersonMail { get; set; }
        [DisplayName("職業")]
        public string txtPersonJob { get; set; }
        [DisplayName("性別")]
        public string labPersonSex { get; set; }


    }
}
