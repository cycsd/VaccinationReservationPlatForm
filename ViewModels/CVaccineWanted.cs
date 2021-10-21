using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CVaccineWanted
    {
        

        [DisplayName("身分證字號")]
        [Required(ErrorMessage = "身分證字號為必填欄位")]
        [RegularExpression(@"^[A-Za-z]{1}[0-9]{9}$", ErrorMessage = "身分證號碼格式錯誤,請重新輸入")]
        [StringLength(10)]
        public string txtPersonIdentityID { get; set; }

        [DisplayName("姓名")]
        public string txtPersonName { get; set; }

        [DisplayName("手機號碼")]
        public string txtPhoneNumber { get; set; }

        public string ddlCountyName { get; set; }

        public string ddlCountyTownName { get; set; }

        public int cbVaccineWanted { get; set; }
    }
}
