using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.Controllers.NumberInfo
{
    public class NumberInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HospitalNameList([FromQuery] int? zipcode)
        {
            VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();
            if (zipcode != null)
            {
                var HP = from h in db.Hospitals
                         where h.CountyPostalCode == zipcode
                         select new SelectListItem
                         {
                             Text = h.HospitalName + " (" + h.HospitalAdress + ")",
                             Value = h.HospitalId.ToString()
                         };

                ViewData["hospitalList"] = HP;
            }
            return PartialView("_HospitalNameList");
        }
        public IActionResult GetHNumber([FromQuery] string selectHospital)
        {
            VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();

            var VB = from b in db.VaccinationBookings
                     where b.VbcheckRemark == "登記"
                     orderby b.Vbnumber
                     select b.Vbnumber;
            ViewBag.Num = VB.FirstOrDefault();

            return PartialView("_GetHNumber");
        }
    }
}
