using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class Disease
    {
        public Disease()
        {
            UserInfos = new HashSet<UserInfo>();
            VaccineToDiseases = new HashSet<VaccineToDisease>();
        }

        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public int? DiseaseCategoryId { get; set; }

        public virtual DiseaseCategory DiseaseCategory { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
        public virtual ICollection<VaccineToDisease> VaccineToDiseases { get; set; }
    }
}
