using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class UserInfo
    {
        public int UserInfoId { get; set; }
        public int? PersonId { get; set; }
        public int? DiseaseId { get; set; }
        public string UserInfoDiseaseDetail { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Person Person { get; set; }
    }
}
