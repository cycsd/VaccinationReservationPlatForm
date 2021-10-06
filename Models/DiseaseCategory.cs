using System;
using System.Collections.Generic;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class DiseaseCategory
    {
        public DiseaseCategory()
        {
            Diseases = new HashSet<Disease>();
        }

        public int DiseaseCategoryId { get; set; }
        public string DiseaseCategoryName { get; set; }

        public virtual ICollection<Disease> Diseases { get; set; }
    }
}
