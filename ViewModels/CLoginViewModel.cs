using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class CLoginViewModel
    {
        [DisplayName("身分證字號")]
        [Required(ErrorMessage = "身分證字號為必填欄位")]
        [RegularExpression(@"^[A-Za-z]{1}[0-9]{9}$", ErrorMessage = "身分證號碼格式錯誤,請重新輸入")]
        [StringLength(10)]
        public string txtPersonIdentityID { get; set; }

        [DisplayName("健保卡卡號")]
        [Required(ErrorMessage = "健保卡卡號為必填欄位")]
        [StringLength(12)]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "健保卡卡號格式錯誤,請重新輸入")]
        public string txtPersonHealthID { get; set; }
    }
}
