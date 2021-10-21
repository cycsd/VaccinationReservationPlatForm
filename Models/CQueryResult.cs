using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Models
{
    public class CQueryResult
    {
        //p.PersonIdentityId,
        //p.PersonName,
        //p.PersonCellphoneNumber,
        //n.CountyTownName,
        //n.CountyName,
        //v.VaccineName,
        [Key]
        public int VaccineId { get; set; }
        [DisplayName("身分證號")]
        public string PersonIdentityId { get; set; }
        [DisplayName("姓名")]
        public string PersonName { get; set; }
        [DisplayName("手機號碼")]
        public string PersonCellphoneNumber { get; set; }
        public string CountyTownName { get; set; }
        [DisplayName("接踵區域")]
        public string CountyName { get; set; }
        [DisplayName("疫苗種類")]
        public string VaccineName { get; set; }
    }
}
